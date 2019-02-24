using PortolMobile.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Portol.DTO;
using Portol.Common.Helper;
using System.Net.Http;
using Portol.Common.Interfaces.PortolMobile;

namespace PortolMobile.Services.User
{
  
    public class LoginService: ILoginService
    {
        private readonly IRestClient _restClient;

        public LoginService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        
        public async Task<UserDto> Authenticate(string email, string password)
        {
            try
            {
                var user = new UserDto { Email = email, Password = password };
                user = await _restClient.MakeApiCall<UserDto>($"{Constants.BaseUserApiUrl}/authenticate", HttpMethod.Post, user);
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
