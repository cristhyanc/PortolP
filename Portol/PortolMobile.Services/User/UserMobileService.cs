using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using Portol.DTO;
using PortolMobile.Services.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Services.User
{
   public class UserMobileService: IUserMobileService
    {
        private readonly IRestClient _restClient;

        public UserMobileService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<Boolean> CreateNewuser(UserDto newUser)
        {
            return await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/RegisterNewuser", HttpMethod.Post, newUser);           
        }
    }
}
