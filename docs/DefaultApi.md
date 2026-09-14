# MaytesApiClient.Api.DefaultApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CancelCheckout**](DefaultApi.md#cancelcheckout) | **POST** /api/merchant/v1/checkouts/{checkoutUuid}/cancel | Cancel authorized checkout |
| [**CaptureCheckout**](DefaultApi.md#capturecheckout) | **POST** /api/merchant/v1/checkouts/{checkoutUuid}/capture | Capture authorized checkout |
| [**CreateCheckout**](DefaultApi.md#createcheckout) | **POST** /api/merchant/v1/checkouts | Create a new checkout |
| [**GetCheckout**](DefaultApi.md#getcheckout) | **GET** /api/merchant/v1/checkouts/{checkoutUuid} | Get checkout details |
| [**GetHealth**](DefaultApi.md#gethealth) | **GET** /api/health | Health check |
| [**GetOAuthToken**](DefaultApi.md#getoauthtoken) | **POST** /oauth/token | Issue an access token |
| [**GetSettlement**](DefaultApi.md#getsettlement) | **GET** /api/merchant/v1/settlements/{uuid} | Get a settlement by UUID |
| [**ListSettlements**](DefaultApi.md#listsettlements) | **GET** /api/merchant/v1/settlements | List settlements for this merchant |
| [**RefundCheckout**](DefaultApi.md#refundcheckout) | **POST** /api/merchant/v1/checkouts/{checkoutUuid}/refund | Refund a captured checkout |

<a id="cancelcheckout"></a>
# **CancelCheckout**
> CancelCheckoutResponse CancelCheckout (string checkoutUuid, CancelCheckoutRequest cancelCheckoutRequest)

Cancel authorized checkout

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class CancelCheckoutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var checkoutUuid = "checkoutUuid_example";  // string | 
            var cancelCheckoutRequest = new CancelCheckoutRequest(); // CancelCheckoutRequest | 

            try
            {
                // Cancel authorized checkout
                CancelCheckoutResponse result = apiInstance.CancelCheckout(checkoutUuid, cancelCheckoutRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CancelCheckout: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CancelCheckoutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Cancel authorized checkout
    ApiResponse<CancelCheckoutResponse> response = apiInstance.CancelCheckoutWithHttpInfo(checkoutUuid, cancelCheckoutRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CancelCheckoutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **checkoutUuid** | **string** |  |  |
| **cancelCheckoutRequest** | [**CancelCheckoutRequest**](CancelCheckoutRequest.md) |  |  |

### Return type

[**CancelCheckoutResponse**](CancelCheckoutResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |
| **404** |  |  -  |
| **409** |  |  -  |
| **422** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="capturecheckout"></a>
# **CaptureCheckout**
> CaptureCheckoutResponse CaptureCheckout (string checkoutUuid, CaptureCheckoutRequest captureCheckoutRequest)

Capture authorized checkout

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class CaptureCheckoutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var checkoutUuid = "checkoutUuid_example";  // string | 
            var captureCheckoutRequest = new CaptureCheckoutRequest(); // CaptureCheckoutRequest | 

            try
            {
                // Capture authorized checkout
                CaptureCheckoutResponse result = apiInstance.CaptureCheckout(checkoutUuid, captureCheckoutRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CaptureCheckout: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CaptureCheckoutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Capture authorized checkout
    ApiResponse<CaptureCheckoutResponse> response = apiInstance.CaptureCheckoutWithHttpInfo(checkoutUuid, captureCheckoutRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CaptureCheckoutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **checkoutUuid** | **string** |  |  |
| **captureCheckoutRequest** | [**CaptureCheckoutRequest**](CaptureCheckoutRequest.md) |  |  |

### Return type

[**CaptureCheckoutResponse**](CaptureCheckoutResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |
| **404** |  |  -  |
| **409** |  |  -  |
| **422** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createcheckout"></a>
# **CreateCheckout**
> CreateCheckoutResponse CreateCheckout (CreateCheckoutRequest createCheckoutRequest)

Create a new checkout

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class CreateCheckoutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var createCheckoutRequest = new CreateCheckoutRequest(); // CreateCheckoutRequest | 

            try
            {
                // Create a new checkout
                CreateCheckoutResponse result = apiInstance.CreateCheckout(createCheckoutRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CreateCheckout: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateCheckoutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new checkout
    ApiResponse<CreateCheckoutResponse> response = apiInstance.CreateCheckoutWithHttpInfo(createCheckoutRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CreateCheckoutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createCheckoutRequest** | [**CreateCheckoutRequest**](CreateCheckoutRequest.md) |  |  |

### Return type

[**CreateCheckoutResponse**](CreateCheckoutResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |
| **409** |  |  -  |
| **422** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getcheckout"></a>
# **GetCheckout**
> GetCheckoutResponse GetCheckout (string checkoutUuid, string? merchantOrderId = null)

Get checkout details

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class GetCheckoutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var checkoutUuid = "checkoutUuid_example";  // string | 
            var merchantOrderId = "merchantOrderId_example";  // string? |  (optional) 

            try
            {
                // Get checkout details
                GetCheckoutResponse result = apiInstance.GetCheckout(checkoutUuid, merchantOrderId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.GetCheckout: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCheckoutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get checkout details
    ApiResponse<GetCheckoutResponse> response = apiInstance.GetCheckoutWithHttpInfo(checkoutUuid, merchantOrderId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.GetCheckoutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **checkoutUuid** | **string** |  |  |
| **merchantOrderId** | **string?** |  | [optional]  |

### Return type

[**GetCheckoutResponse**](GetCheckoutResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |
| **404** |  |  -  |
| **422** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="gethealth"></a>
# **GetHealth**
> HealthResponse GetHealth ()

Health check

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class GetHealthExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);

            try
            {
                // Health check
                HealthResponse result = apiInstance.GetHealth();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.GetHealth: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetHealthWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Health check
    ApiResponse<HealthResponse> response = apiInstance.GetHealthWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.GetHealthWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**HealthResponse**](HealthResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getoauthtoken"></a>
# **GetOAuthToken**
> OAuthTokenResponse GetOAuthToken (OAuthTokenRequest oAuthTokenRequest)

Issue an access token

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class GetOAuthTokenExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var oAuthTokenRequest = new OAuthTokenRequest(); // OAuthTokenRequest | 

            try
            {
                // Issue an access token
                OAuthTokenResponse result = apiInstance.GetOAuthToken(oAuthTokenRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.GetOAuthToken: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetOAuthTokenWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Issue an access token
    ApiResponse<OAuthTokenResponse> response = apiInstance.GetOAuthTokenWithHttpInfo(oAuthTokenRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.GetOAuthTokenWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **oAuthTokenRequest** | [**OAuthTokenRequest**](OAuthTokenRequest.md) |  |  |

### Return type

[**OAuthTokenResponse**](OAuthTokenResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **400** | Invalid grant or request |  -  |
| **401** | Unauthorized |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getsettlement"></a>
# **GetSettlement**
> GetSettlementResponse GetSettlement (string uuid)

Get a settlement by UUID

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class GetSettlementExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var uuid = "uuid_example";  // string | Settlement UUID

            try
            {
                // Get a settlement by UUID
                GetSettlementResponse result = apiInstance.GetSettlement(uuid);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.GetSettlement: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSettlementWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a settlement by UUID
    ApiResponse<GetSettlementResponse> response = apiInstance.GetSettlementWithHttpInfo(uuid);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.GetSettlementWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **uuid** | **string** | Settlement UUID |  |

### Return type

[**GetSettlementResponse**](GetSettlementResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |
| **404** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="listsettlements"></a>
# **ListSettlements**
> ListSettlementsResponse ListSettlements (Object? offset = null, Object? limit = null, string? sortDir = null, string? sortBy = null, Object? createdBefore = null, Object? createdAfter = null, string? status = null)

List settlements for this merchant

Returns settlements for the authenticated merchant. Default sort: created_at ASC.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class ListSettlementsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var offset = new Object?(); // Object? | Pagination offset (default 0) (optional) 
            var limit = new Object?(); // Object? | Max records to return (default 50, max 200) (optional) 
            var sortDir = "asc";  // string? | Sort direction (default: asc) (optional) 
            var sortBy = "created_at";  // string? | Field to sort by (default: created_at) (optional) 
            var createdBefore = new Object?(); // Object? | Include settlements created at or before this ISO-8601 timestamp (optional) 
            var createdAfter = new Object?(); // Object? | Include settlements created at or after this ISO-8601 timestamp (optional) 
            var status = "CREATED";  // string? | Filter by status (optional) 

            try
            {
                // List settlements for this merchant
                ListSettlementsResponse result = apiInstance.ListSettlements(offset, limit, sortDir, sortBy, createdBefore, createdAfter, status);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ListSettlements: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ListSettlementsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // List settlements for this merchant
    ApiResponse<ListSettlementsResponse> response = apiInstance.ListSettlementsWithHttpInfo(offset, limit, sortDir, sortBy, createdBefore, createdAfter, status);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ListSettlementsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **offset** | [**Object?**](Object?.md) | Pagination offset (default 0) | [optional]  |
| **limit** | [**Object?**](Object?.md) | Max records to return (default 50, max 200) | [optional]  |
| **sortDir** | **string?** | Sort direction (default: asc) | [optional]  |
| **sortBy** | **string?** | Field to sort by (default: created_at) | [optional]  |
| **createdBefore** | [**Object?**](Object?.md) | Include settlements created at or before this ISO-8601 timestamp | [optional]  |
| **createdAfter** | [**Object?**](Object?.md) | Include settlements created at or after this ISO-8601 timestamp | [optional]  |
| **status** | **string?** | Filter by status | [optional]  |

### Return type

[**ListSettlementsResponse**](ListSettlementsResponse.md)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |
| **401** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="refundcheckout"></a>
# **RefundCheckout**
> void RefundCheckout (string checkoutUuid, CreateRefundRequest createRefundRequest)

Refund a captured checkout

Initiates a full refund of the captured payment. Returns 204 immediately; Stripe confirms asynchronously via webhook. Poll GET /checkouts/:uuid for status and refund details.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MaytesApiClient.Api;
using MaytesApiClient.Client;
using MaytesApiClient.Model;

namespace Example
{
    public class RefundCheckoutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: bearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new DefaultApi(httpClient, config, httpClientHandler);
            var checkoutUuid = "checkoutUuid_example";  // string | 
            var createRefundRequest = new CreateRefundRequest(); // CreateRefundRequest | 

            try
            {
                // Refund a captured checkout
                apiInstance.RefundCheckout(checkoutUuid, createRefundRequest);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.RefundCheckout: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RefundCheckoutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Refund a captured checkout
    apiInstance.RefundCheckoutWithHttpInfo(checkoutUuid, createRefundRequest);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.RefundCheckoutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **checkoutUuid** | **string** |  |  |
| **createRefundRequest** | [**CreateRefundRequest**](CreateRefundRequest.md) |  |  |

### Return type

void (empty response body)

### Authorization

[bearer](../README.md#bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** |  |  -  |
| **401** |  |  -  |
| **402** |  |  -  |
| **404** |  |  -  |
| **409** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

