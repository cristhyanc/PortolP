using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Services.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Services.User
{
  public  class AddressService: IAddressService
    {

        private readonly IRestClient _restClient;
        private string _addressKey, _addressSecret;

        public AddressService(IRestClient restClient, string addressKey, string addressSecret)
        {
            _restClient = restClient;
            _addressKey = addressKey;
            _addressSecret = addressSecret;
        }
        public async Task<AddressFinderDto> GetPosibleAddresses(AddressDto tentativeAddress)
        {
            string query = "format=json&key=" + _addressKey + "&secret=" + _addressSecret + "&domain=" + Constants.PortolDomain;
            query += "&q=" + tentativeAddress.FullAddress;
            return await _restClient.MakeApiCall<AddressFinderDto>($"{Constants.BaseAddressApiUrl}/autocomplete", HttpMethod.Get, query);
        }

        public async Task<AddressFinderDetail> GetAddressMetadata(string addressId)
        {
            string query = "format=json&key=" + _addressKey + "&secret=" + _addressSecret + "&domain=" + Constants.PortolDomain;
            query += "&id=" + addressId;
            return await _restClient.MakeApiCall<AddressFinderDetail>($"{Constants.BaseAddressApiUrl}/info", HttpMethod.Get, query);
        }
    }
}
