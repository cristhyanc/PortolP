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

    public class LoginService : ILoginService
    {
        private readonly IRestClient _restClient;

        public LoginService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<UserDto> Authenticate(string email, string password)
        {            
                var user = new UserDto { Email = email, Password = password };
                user = await _restClient.MakeApiCall<UserDto>($"{Constants.BaseUserApiUrl}/authenticate", HttpMethod.Post, user);
                return user; 
        }

        public async Task<bool> VerifyCode(decimal mobilePhoned, Int32 code)
        {
            UserDto user = new UserDto { PhoneNumber = mobilePhoned, PhoneCountryCode = code };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/VerifyCode", HttpMethod.Post, user);
        }

        public async Task<bool> ResetNewPassword(decimal mobilePhoned, string newPassword)
        {
            UserDto user = new UserDto { PhoneNumber = mobilePhoned, Password= newPassword };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/ResetPassword", HttpMethod.Post, user);
        }

        public async Task<bool> SendVerificationCode(decimal mobilePhoned, Int32 code)
        {
            UserDto user = new UserDto { PhoneNumber = mobilePhoned, PhoneCountryCode= code };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/SendVerificationCode", HttpMethod.Post, user);
        }

    }
}
