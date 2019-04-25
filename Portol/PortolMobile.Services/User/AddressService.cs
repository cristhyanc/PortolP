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

        public async Task<AddressFinderDto> GetPosibleAddresses(string address)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["format"] = "json";
            queryString["key"] = _addressKey;
            queryString["secret"] = _addressSecret;
            queryString["domain"] = Constants.PortolDomain;
            queryString["q"] = address;
          
            return await _restClient.MakeApiCall<AddressFinderDto>($"{Constants.BaseAddressApiUrl}/autocomplete", HttpMethod.Get, queryString.ToString());
        }

        public async Task<AddressFinderDetail> GetAddressMetadata(string addressId)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["format"] = "json";
            queryString["key"] = _addressKey;
            queryString["secret"] = _addressSecret;
            queryString["domain"] = Constants.PortolDomain;
            queryString["id"] = addressId;
            
            return await _restClient.MakeApiCall<AddressFinderDetail>($"{Constants.BaseAddressApiUrl}/info", HttpMethod.Get, queryString.ToString());
        }
    }
}
