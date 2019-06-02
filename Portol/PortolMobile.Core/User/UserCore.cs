using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.User
{
   public class UserCore: IUserCore
    {
        IUserMobileService _userMobileService;
        public UserCore(IUserMobileService userMobileService)
        {
            _userMobileService = userMobileService;
        }

        public async Task<Boolean> SavePaymentMethod(PaymentMethodDto paymentMethod)
        {
            return await _userMobileService.SavePaymentMethod(paymentMethod);
        }

        public async Task<bool> ResetNewPassword(long mobilePhoned, string newPassword)
        {
            return await _userMobileService.ResetNewPassword(mobilePhoned, newPassword);
        }
       
        public async Task<bool> SendVerificationCode(long mobilePhoned, Int32 code)
        {
            return await _userMobileService.SendVerificationCode(mobilePhoned, code);
        }

        public async Task<bool> VerifyCode(long mobilePhoned, Int32 countryCode, Int32 code)
        {
            return await _userMobileService.VerifyCode(mobilePhoned, countryCode, code);
        }

        public async Task<Boolean> CreateNewCustomer(CustomerDto newUser)
        {
            return await _userMobileService.CreateNewCustomer(newUser);
        }

        public async Task<CustomerDto> GetCustomerByEmail(string email)
        {
            return await _userMobileService.GetCustomerByEmail(email);
        }

        public async Task<CustomerDto> GetCustomer(Guid customerID)
        {
            return await _userMobileService.GetCustomer(customerID);
        }
        public async Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {
            return await _userMobileService.GetCustomerByPhoneNumber(phoneNumber, countryCode);
        }

        public async Task<Boolean> VerifyEmailUniqueness(string email)
        {
            return await _userMobileService.VerifyEmailUniqueness(email);
        }

        public async Task<Boolean> VerifyMobileUniqueness(long mobilePhoned, Int32 code)
        {
            return await _userMobileService.VerifyMobileUniqueness(mobilePhoned, code);
        }

        public async Task<Boolean> SaveCustomer(CustomerDto user)
        {
            return await _userMobileService.SaveCustomer(user);
        }
    }
}
