# MaytesApiClient.Model.CreateCheckoutRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**MerchantOrderId** | **string** |  | [optional] 
**MerchantExpiresAt** | **DateTime?** |  | [optional] 
**TotalAmount** | **long** |  | 
**Currency** | **string** |  | 
**ReturnUrl** | **string** |  | 
**CancelUrl** | **string** |  | 
**Items** | [**List&lt;LineItem&gt;**](LineItem.md) |  | 
**Fees** | [**List&lt;CheckoutFee&gt;**](CheckoutFee.md) |  | [optional] 
**Discounts** | [**List&lt;CheckoutDiscount&gt;**](CheckoutDiscount.md) |  | [optional] 
**Tax** | [**Tax**](Tax.md) |  | [optional] 
**ShippingAmount** | **int?** |  | [optional] 
**CustomerData** | [**CustomerData**](CustomerData.md) |  | [optional] 
**Metadata** | **Dictionary&lt;string, Object&gt;** |  | [optional] 
**AllocationModel** | **string** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

