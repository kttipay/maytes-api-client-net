// Bundled with the generated SDK via bin/postprocess-dotnet.sh.
//
// Lives at build/dotnet-client/src/MaytesApiClient/Auth/ClientFactory.cs after
// `make generate-dotnet`. The generated .csproj globs *.cs under
// src/MaytesApiClient/ by default, so the file is picked up automatically.
//
// Adds OAuth2 client_credentials credential management on top of the raw
// DefaultApi using a **reactive 401-retry** pattern:
//
//   1. First authed call mints a token via GetOAuthToken (a @auth([]) op) and
//      caches it.
//   2. Subsequent calls send the cached token as-is — no client-side expiry
//      math, no proactive refresh.
//   3. If the server returns 401, the DelegatingHandler refreshes the token,
//      clones the original request with the new Authorization header, and
//      sends it once more. If the retry also 401s, the second response
//      propagates.
//
// Concurrent 401s deduplicate via RefreshIfStaleAsync(staleToken): only the
// first caller to acquire the SemaphoreSlim mints; others observe the cache
// already updated and return that token.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace MaytesApiClient.Auth
{
    /// <summary>
    /// Caches an OAuth2 client_credentials token; refreshes only on demand.
    /// Threadsafe — concurrent refreshes serialize behind a SemaphoreSlim and
    /// deduplicate via the stale-token snapshot.
    /// </summary>
    public sealed class OAuthTokenProvider
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly DefaultApi _unauthedApi;
        private readonly SemaphoreSlim _mintLock = new SemaphoreSlim(1, 1);

        /// <summary>The last minted token, or null if nothing has been minted yet.</summary>
        public string? CachedToken { get; private set; }

        /// <summary>
        /// Creates a provider that mints OAuth2 tokens against
        /// <paramref name="endpoint"/>. The optional <paramref name="initialToken"/>
        /// seeds the cache with a pre-minted JWT (used by the E2E test path to
        /// exercise the reactive 401-retry behavior).
        /// </summary>
        public OAuthTokenProvider(string endpoint, string clientId, string clientSecret, string? initialToken = null)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            CachedToken = initialToken;

            // Separate unauthed api used only for GetOAuthToken (a @auth([]) op).
            // Never recurses into itself for a token.
            var unauthedConfig = new Configuration { BasePath = endpoint };
            _unauthedApi = new DefaultApi(unauthedConfig);
        }

        /// <summary>Returns the cached token, minting one first if the cache is empty.</summary>
        public async Task<string> GetAsync(CancellationToken cancellationToken = default)
        {
            var current = CachedToken;
            if (current != null) return current;

            await _mintLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (CachedToken != null) return CachedToken;
                return await MintLockedAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _mintLock.Release();
            }
        }

        /// <summary>
        /// Mint a new token only if the cache still holds the stale one the
        /// caller observed. If another thread already refreshed, return that
        /// newer token instead — no second mint.
        /// </summary>
        public async Task<string> RefreshIfStaleAsync(string staleToken, CancellationToken cancellationToken = default)
        {
            await _mintLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (CachedToken != null && CachedToken != staleToken)
                {
                    return CachedToken;
                }
                return await MintLockedAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _mintLock.Release();
            }
        }

        private async Task<string> MintLockedAsync(CancellationToken cancellationToken)
        {
            // `client_id` is `format: uuid` in the spec, so the generated model types it as Guid.
            var body = new OAuthTokenRequest(
                grantType: "client_credentials",
                clientId: Guid.Parse(_clientId),
                clientSecret: _clientSecret);
            var response = await _unauthedApi.GetOAuthTokenAsync(body, cancellationToken).ConfigureAwait(false);
            CachedToken = response.AccessToken;
            return CachedToken!;
        }
    }

    /// <summary>
    /// DelegatingHandler that attaches Bearer auth and reactively refreshes on
    /// 401. The retry uses a buffered copy of the request body so it can be
    /// replayed verbatim (HttpRequestMessage instances are single-use).
    /// </summary>
    internal sealed class BearerAuthHandler : DelegatingHandler
    {
        private readonly OAuthTokenProvider _provider;

        public BearerAuthHandler(OAuthTokenProvider provider, HttpMessageHandler inner)
        {
            _provider = provider;
            InnerHandler = inner;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath;
            if (path == "/api/health" || path == "/oauth/token")
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            // Buffer body so we can resend on retry. HttpContent streams may be
            // unseekable single-use; LoadIntoBufferAsync materialises it into
            // memory so cloning later can re-read it.
            if (request.Content != null)
            {
                await request.Content.LoadIntoBufferAsync().ConfigureAwait(false);
            }

            var token = await _provider.GetAsync(cancellationToken).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            response.Dispose();
            var newToken = await _provider.RefreshIfStaleAsync(token, cancellationToken).ConfigureAwait(false);

            var retry = await CloneRequestAsync(request).ConfigureAwait(false);
            retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
            return await base.SendAsync(retry, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
        {
            var clone = new HttpRequestMessage(original.Method, original.RequestUri);
            foreach (var header in original.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (original.Content != null)
            {
                var ms = new MemoryStream();
                await original.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);
                foreach (var header in original.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            return clone;
        }
    }

    /// <summary>
    /// Factory for fully managed Maytes API clients.
    ///
    ///   var api = ClientFactory.Create(endpoint, clientId, clientSecret);
    ///   var result = await api.CreateCheckoutAsync(body);
    ///
    /// <c>initialToken</c> is an SDK-test-only escape hatch — seeds the cache
    /// with a pre-minted JWT, used by the E2E test path to verify the
    /// wrapper's reactive 401-retry refresh when the seeded token expires.
    /// Production callers omit it.
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        /// Creates a managed <see cref="DefaultApi"/> that handles OAuth2 token
        /// minting + reactive 401-retry transparently. See the XML on the
        /// <c>ClientFactory</c> type for the full contract.
        /// </summary>
        public static DefaultApi Create(string endpoint, string clientId, string clientSecret, string? initialToken = null)
        {
            var provider = new OAuthTokenProvider(endpoint, clientId, clientSecret, initialToken);
            var innerHandler = new HttpClientHandler();
            var bearerHandler = new BearerAuthHandler(provider, innerHandler);
            var httpClient = new HttpClient(bearerHandler) { BaseAddress = new Uri(endpoint) };

            var config = new Configuration { BasePath = endpoint };
            return new DefaultApi(httpClient, config, innerHandler);
        }
    }
}
