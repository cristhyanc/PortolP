using PortolMobile.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Portol.Common.DTO;
using Portol.Common.Helper;
using System.Net.Http;
using Portol.Common.Interfaces.PortolMobile;
using System.Collections.Specialized;

namespace PortolMobile.Services.User
{

    public class LoginService : ILoginService
    {
        private readonly IRestClient _restClient;

        public LoginService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<CustomerDto> Authenticate(string email, string password)
        {            
                var user = new CustomerDto { Email = email, Password = password };
                user = await _restClient.MakeApiCall<CustomerDto>($"{Constants.BaseUserApiUrl}/authenticate", HttpMethod.Post, user);           
                return user; 
        }

        public async Task<MetadataDto> GetMetadata(string appKey)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["metadatakey"] = appKey.ToString();            
            var result = await _restClient.MakeApiCall<MetadataDto>($"{Constants.BaseUserApiUrl}/GetMetaData", HttpMethod.Get , queryString);
            return result;
        }


    }
}
