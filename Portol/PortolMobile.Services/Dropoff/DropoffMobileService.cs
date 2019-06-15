using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Services.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Services.Dropoff
{
    public class DropoffMobileService : IDeliveryMobileService
    {
        private readonly IRestClient _restClient;
        public DropoffMobileService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<DeliveryDto> GetSendertDeliveryInProgress(Guid customerID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["customerID"] = customerID.ToString();
            return await _restClient.MakeApiCall<DeliveryDto>($"{Constants.BaseDropoffApiUrl}/GetSendertDeliveryInProgress", HttpMethod.Get, queryString.ToString());
        }
        
        public async Task<DeliveryStatus> GetDeliveryStatus(Guid deliveryID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();
            return await _restClient.MakeApiCallRaw<DeliveryStatus>($"{Constants.BaseDropoffApiUrl}/GetDeliveryStatus", HttpMethod.Get, queryString.ToString());
        }

        public async Task<bool> ConfirmDeliveryPickUp(Guid deliveryID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();
            return await _restClient.MakeApiCallRaw<bool>($"{Constants.BaseDropoffApiUrl}/ConfirmDeliveryPickUp", HttpMethod.Get, queryString.ToString());
        }

        public async Task<bool> CancelDelivery(Guid deliveryID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();          
            return await _restClient.MakeApiCallRaw<bool>($"{Constants.BaseDropoffApiUrl}/CancelDelivery", HttpMethod.Get, queryString.ToString());
        }

        public async Task<bool> RateDelivery(Guid deliveryID, int rate)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();
            queryString["rate"] = rate.ToString();
            return await _restClient.MakeApiCallRaw<bool>($"{Constants.BaseDropoffApiUrl}/RateDelivery", HttpMethod.Get, queryString.ToString());
        }

        public async Task<bool> MarkAsDelivered(Guid deliveryID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();           
            return await _restClient.MakeApiCallRaw<bool>($"{Constants.BaseDropoffApiUrl}/MarkAsDelivered", HttpMethod.Get, queryString.ToString());
        }

        public async Task<DriverDto> GetDeliveryDriverInfo(Guid deliveryID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["deliveryID"] = deliveryID.ToString();
            return await _restClient.MakeApiCall<DriverDto>($"{Constants.BaseDropoffApiUrl}/GetDeliveryDriverInfo", HttpMethod.Get, queryString.ToString());
        }

        public async Task<List<VehiculeTypeDto>> GetVehiculeTypesAvailables()
        {           
            return await _restClient.MakeApiCall<List<VehiculeTypeDto>>($"{Constants.BaseDropoffApiUrl}/GetVehiculeTypesAvailables", HttpMethod.Get, null);
        }

        public async Task<List<DeliveryDto>> GetPendingReceiverDeliveries(Guid receiverID)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["receiverID"] = receiverID.ToString();
            return await _restClient.MakeApiCall<List<DeliveryDto>>($"{Constants.BaseDropoffApiUrl}/GetPendingReceiverDeliveries", HttpMethod.Get, queryString.ToString());
        }
        public async Task<List<DeliveryDto>> GetSentDeliveriesByCustomer(Guid customerId)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["customerId"] = customerId.ToString();
            return await _restClient.MakeApiCall<List<DeliveryDto>>($"{Constants.BaseDropoffApiUrl}/GetSentDeliveriesByCustomer", HttpMethod.Get, queryString.ToString());
        }
        public async Task<Guid> CreateDeliveryRequest(DeliveryDto dropoffParcel)
        {
            return await _restClient.MakeApiCallRaw<Guid>($"{Constants.BaseDropoffApiUrl}/CreateDropoffRequest", HttpMethod.Post , dropoffParcel);
        }
    }
}
