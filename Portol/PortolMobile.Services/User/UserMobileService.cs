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
   public class UserMobileService: ICustomerMobileService
    {
        private readonly IRestClient _restClient;

        public UserMobileService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<Boolean> CreateNewCustomer(CustomerDto newUser)
        {
            return await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/RegisterNewuser", HttpMethod.Post, newUser);           
        }
    }
}
