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

        public async Task<Guid> CreateDeliveryRequest(DeliveryDto dropoffParcel)
        {
            return await _restClient.MakeApiCallRaw<Guid>($"{Constants.BaseDropoffApiUrl}/CreateDropoffRequest", HttpMethod.Post , dropoffParcel);
        }
    }
}
