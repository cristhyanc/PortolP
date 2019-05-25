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
   public class UserMobileService: IUserMobileService
    {
        private readonly IRestClient _restClient;

        public UserMobileService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<Boolean> SaveCustomer(CustomerDto user)
        {
            return await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/SaveUser", HttpMethod.Post, user);           
        }

        public async Task<Boolean> SavePaymentMethod(PaymentMethodDto paymentMethod)
        {
            return await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/SavePaymentMethod", HttpMethod.Post, paymentMethod);
        }

        public async Task<Boolean> CreateNewCustomer(CustomerDto newUser)
        {
            return await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/RegisterNewuser", HttpMethod.Post, newUser);
        }
        public async Task<CustomerDto> GetCustomerByEmail(string email)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["email"] = email;
            return await _restClient.MakeApiCall<CustomerDto>($"{Constants.BaseUserApiUrl}/GetCustomerByEmail", HttpMethod.Get, queryString);
        }

        public async Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["phoneNumber"] = phoneNumber.ToString();
            queryString["countryCode"] = countryCode.ToString();

            return await _restClient.MakeApiCall<CustomerDto>($"{Constants.BaseUserApiUrl}/GetCustomerByPhoneNumber", HttpMethod.Get, queryString);
        }

        public async Task<Boolean> VerifyMobileUniqueness(long mobilePhoned, Int32 code)
        {
            CustomerDto details = new CustomerDto { PhoneNumber = mobilePhoned, PhoneCountryCode = code };
            var result = await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/VerifyMobileUniqueness", HttpMethod.Post, details);
            return result;
        }

        public async Task<Boolean> VerifyEmailUniqueness(string email)
        {
            var result = await _restClient.MakeApiCallRaw<Boolean>($"{Constants.BaseUserApiUrl}/VerifyEmailUniqueness", HttpMethod.Post, email);
            return result;
        }

        public async Task<bool> VerifyCode(long mobilePhoned, Int32 countryCode, Int32 code)
        {
            CustomerDto user = new CustomerDto { PhoneNumber = mobilePhoned, PhoneCountryCode = countryCode, Token = code.ToString() };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/VerifyCode", HttpMethod.Post, user);
        }

        public async Task<bool> ResetNewPassword(long mobilePhoned, string newPassword)
        {
            CustomerDto user = new CustomerDto { PhoneNumber = mobilePhoned, Password = newPassword };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/ResetPassword", HttpMethod.Post, user);
        }

        public async Task<bool> SendVerificationCode(long mobilePhoned, Int32 code)
        {
            CustomerDto user = new CustomerDto { PhoneNumber = mobilePhoned, PhoneCountryCode = code };
            return await _restClient.MakeApiCall($"{Constants.BaseUserApiUrl}/SendVerificationCode", HttpMethod.Post, user);
        }

    }
}
