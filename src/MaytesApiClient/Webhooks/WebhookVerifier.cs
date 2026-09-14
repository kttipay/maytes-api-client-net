// Bundled with the generated SDK via bin/postprocess-dotnet.sh.
//
// Lives at build/dotnet-client/src/MaytesApiClient/Webhooks/WebhookVerifier.cs
// after `make generate-dotnet`. The generated .csproj globs *.cs under
// src/MaytesApiClient/ by default, so the file is picked up automatically.
//
// Verifies the `X-Maytes-Signature` header on an inbound webhook delivery and
// returns the parsed event. Merchants must never hand-roll this — see the four
// sharp edges below, each of which silently produces either a security hole or
// an outage:
//
//   1. RAW BODY. The signature covers the exact bytes we sent. Verifying
//      against re-serialised JSON fails for any body whose key order or number
//      formatting differs after a parse/emit round-trip. This is the single
//      most common webhook integration bug.
//   2. MULTIPLE `v1=` ENTRIES. During a signing-secret rotation grace window
//      the header carries TWO signatures (current secret first, previous
//      second) and the delivery is valid if EITHER matches. A verifier that
//      reads only the first `v1=` works perfectly until the first rotation,
//      then rejects live traffic. See §7 of the webhook spec.
//   3. CONSTANT-TIME COMPARISON. A plain == on digests leaks how much of a
//      forged signature was correct, which is enough to recover a valid
//      signature one byte at a time.
//   4. REPLAY WINDOW. The `t=` timestamp is the anti-replay anchor; a captured
//      delivery stays valid forever without a freshness check.
//
// Verification and JSON parsing are deliberately fused into ONE call: the
// method takes raw bytes and hands back the parsed event, so there is no
// intermediate state in which a caller can parse first and verify second.
//
// Algorithm (webhook spec §6.1):
//   signed_payload = "<t>.<raw body>"
//   signature      = lowercase_hex( HMAC_SHA256(signing_secret, signed_payload) )

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MaytesApiClient.Webhooks
{
    /// <summary>Why a delivery failed verification. Log it; never echo it to the caller.</summary>
    public enum WebhookSignatureErrorReason
    {
        /// <summary>Header absent, or missing its <c>t=</c> / <c>v1=</c> components.</summary>
        MalformedHeader,

        /// <summary><c>t=</c> is further from now than the tolerance allows — possible replay.</summary>
        TimestampOutOfTolerance,

        /// <summary>No <c>v1=</c> entry matched. Wrong secret, or the body was modified.</summary>
        NoMatchingSignature,

        /// <summary>Signature was valid but the body wasn't JSON (should never happen).</summary>
        InvalidJson
    }

    /// <summary>
    /// Thrown when an inbound delivery is not authentic. Respond 400 and do NOT
    /// act on the body.
    /// </summary>
    public class WebhookSignatureException : Exception
    {
        /// <summary>Which check failed. Log it; never echo it to the caller.</summary>
        public WebhookSignatureErrorReason Reason { get; }

        /// <summary>Creates an exception carrying the reason verification failed.</summary>
        /// <param name="reason">Which check failed.</param>
        /// <param name="message">Human-readable detail for your logs.</param>
        public WebhookSignatureException(WebhookSignatureErrorReason reason, string message)
            : base(message)
        {
            Reason = reason;
        }
    }

    /// <summary>
    /// A verified webhook event (webhook spec §6).
    /// </summary>
    /// <remarks>
    /// <see cref="Data"/> is left as an open dictionary because its shape varies
    /// per <see cref="Type"/> — branch on <see cref="Type"/>, then read the fields
    /// that event documents. Ignore unrecognised fields: additive changes do NOT
    /// bump <see cref="ApiVersion"/>.
    /// </remarks>
    public sealed class MaytesWebhookEvent
    {
        /// <summary>Stable event id — identical across every retry. Deduplicate on this.</summary>
        public string Id { get; }

        /// <summary>e.g. <c>checkout.authorized</c>, <c>checkout.voided</c>, <c>webhook.test</c>.</summary>
        public string Type { get; }

        /// <summary>Envelope schema version, e.g. <c>2026-06</c>. Bumped only on breaking changes.</summary>
        public string ApiVersion { get; }

        /// <summary>When the event occurred.</summary>
        public string CreatedAt { get; }

        /// <summary>Per-event payload.</summary>
        public IReadOnlyDictionary<string, JsonElement> Data { get; }

        internal MaytesWebhookEvent(string id, string type, string apiVersion, string createdAt,
            IReadOnlyDictionary<string, JsonElement> data)
        {
            Id = id;
            Type = type;
            ApiVersion = apiVersion;
            CreatedAt = createdAt;
            Data = data;
        }
    }

    /// <summary>
    /// Verifies inbound Maytes webhook deliveries. See <see cref="Verify"/>.
    /// </summary>
    public static class WebhookVerifier
    {
        /// <summary>Default anti-replay window, in seconds (webhook spec §6.1 advises ~5 min).</summary>
        public const int DefaultToleranceSeconds = 300;


        /// <summary>
        /// Verifies an inbound Maytes webhook delivery and returns the parsed event.
        /// </summary>
        /// <remarks>
        /// Pass the <b>raw</b> request body — the exact bytes, before any JSON parsing.
        /// <code>
        /// using MaytesApiClient.Webhooks;
        ///
        /// app.MapPost("/webhooks/maytes", async (HttpRequest request) =>
        /// {
        ///     using var buffer = new MemoryStream();
        ///     await request.Body.CopyToAsync(buffer);
        ///     try
        ///     {
        ///         var evt = WebhookVerifier.Verify(
        ///             buffer.ToArray(),
        ///             request.Headers["X-Maytes-Signature"],
        ///             Environment.GetEnvironmentVariable("MAYTES_WEBHOOK_SECRET"));
        ///         // ... handle evt.Type
        ///         return Results.Ok();
        ///     }
        ///     catch (WebhookSignatureException)
        ///     {
        ///         return Results.BadRequest();
        ///     }
        /// });
        /// </code>
        /// <paramref name="toleranceSeconds"/> = 0 skips the freshness check entirely
        /// — TEST ONLY, for replaying a stored delivery in a fixture. Never 0 in
        /// production: it makes any captured delivery replayable forever.
        /// </remarks>
        /// <exception cref="WebhookSignatureException">If the delivery is not authentic.</exception>
        public static MaytesWebhookEvent Verify(
            byte[] rawBody,
            string? signatureHeader,
            string signingSecret,
            int toleranceSeconds = DefaultToleranceSeconds)
        {
            var (timestamp, signatures) = ParseSignatureHeader(signatureHeader);

            if (toleranceSeconds > 0)
            {
                if (!long.TryParse(timestamp, out var sentAt))
                {
                    throw new WebhookSignatureException(WebhookSignatureErrorReason.MalformedHeader,
                        "X-Maytes-Signature carries a non-numeric t= value.");
                }

                var ageSeconds = Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - sentAt);
                if (ageSeconds > toleranceSeconds)
                {
                    throw new WebhookSignatureException(
                        WebhookSignatureErrorReason.TimestampOutOfTolerance,
                        $"Webhook timestamp is outside the {toleranceSeconds}s tolerance — possible replay.");
                }
            }

            var expected = ComputeExpected(timestamp, rawBody, signingSecret);
            var expectedBytes = Encoding.UTF8.GetBytes(expected);

            // ANY match wins — during a rotation grace window the current secret's
            // signature and the previous secret's signature are both present.
            //
            // Compared as ASCII hex text via FixedTimeEquals rather than decoded
            // bytes: a malformed v1= has no valid decoding. The length guard runs
            // first because FixedTimeEquals short-circuits on differing lengths
            // anyway, and length is not a secret.
            var matched = false;
            foreach (var candidate in signatures)
            {
                var candidateBytes = Encoding.UTF8.GetBytes(candidate);
                if (candidateBytes.Length == expectedBytes.Length
                    && CryptographicOperations.FixedTimeEquals(candidateBytes, expectedBytes))
                {
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                throw new WebhookSignatureException(WebhookSignatureErrorReason.NoMatchingSignature,
                    "No v1 signature matched the computed HMAC — wrong signing secret, "
                    + "or the body was modified in transit.");
            }

            return ParseEnvelope(rawBody);
        }

        private static string ComputeExpected(string timestamp, byte[] rawBody, string signingSecret)
        {
            var prefix = Encoding.UTF8.GetBytes(timestamp + ".");
            var payload = new byte[prefix.Length + rawBody.Length];
            Buffer.BlockCopy(prefix, 0, payload, 0, prefix.Length);
            Buffer.BlockCopy(rawBody, 0, payload, prefix.Length, rawBody.Length);

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingSecret));
            var digest = hmac.ComputeHash(payload);

            var hex = new StringBuilder(digest.Length * 2);
            foreach (var b in digest)
            {
                hex.Append(b.ToString("x2"));
            }
            return hex.ToString();
        }

        private static (string Timestamp, List<string> Signatures) ParseSignatureHeader(string? header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new WebhookSignatureException(WebhookSignatureErrorReason.MalformedHeader,
                    "Missing X-Maytes-Signature header.");
            }

            string? timestamp = null;
            var signatures = new List<string>();

            foreach (var part in header!.Split(','))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("t=", StringComparison.Ordinal))
                {
                    timestamp = trimmed.Substring(2);
                }
                else if (trimmed.StartsWith("v1=", StringComparison.Ordinal))
                {
                    signatures.Add(trimmed.Substring(3));
                }
                // Unknown schemes (a future v2=) are ignored, not fatal — forward
                // compatibility: we may add a scheme alongside v1 before removing v1.
            }

            if (string.IsNullOrEmpty(timestamp) || signatures.Count == 0)
            {
                throw new WebhookSignatureException(WebhookSignatureErrorReason.MalformedHeader,
                    "X-Maytes-Signature is malformed — expected `t=<unix>,v1=<hex>`.");
            }

            return (timestamp!, signatures);
        }

        private static MaytesWebhookEvent ParseEnvelope(byte[] rawBody)
        {
            try
            {
                using var document = JsonDocument.Parse(rawBody);
                var root = document.RootElement;
                if (root.ValueKind != JsonValueKind.Object)
                {
                    throw new JsonException("body is not a JSON object");
                }

                var data = new Dictionary<string, JsonElement>();
                if (root.TryGetProperty("data", out var dataElement)
                    && dataElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in dataElement.EnumerateObject())
                    {
                        data[property.Name] = property.Value.Clone();
                    }
                }

                return new MaytesWebhookEvent(
                    ReadString(root, "id"),
                    ReadString(root, "type"),
                    ReadString(root, "api_version"),
                    ReadString(root, "created_at"),
                    data);
            }
            catch (JsonException)
            {
                throw new WebhookSignatureException(WebhookSignatureErrorReason.InvalidJson,
                    "Signature verified but the body is not valid JSON.");
            }
        }

        private static string ReadString(JsonElement root, string property)
        {
            return root.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.String
                ? value.GetString() ?? string.Empty
                : string.Empty;
        }
    }
}
