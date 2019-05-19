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

        public List<PaymentMethodDto> GetUserPaymentMethods()
        {
            List<PaymentMethodDto> payments = new List<PaymentMethodDto>();
            payments.Add(new PaymentMethodDto { CardName = "Test 1", CardNumber = "456123789456", Country = "AU", CVV = "123", ExpiryDate = "10/20", CreditCardType=Portol.Common.Helper.PaymentMethodType.MasterCard, CurrentCard = true });
            payments.Add(new PaymentMethodDto { CardName = "Test 2", CardNumber = "987456321", Country = "AU", CVV = "654", ExpiryDate = "07/22", CreditCardType = Portol.Common.Helper.PaymentMethodType.Visa});
            payments.Add(new PaymentMethodDto { CardName = "Test 3", CardNumber = "258741369", Country = "AU", CVV = "987", ExpiryDate = "10/17", CreditCardType = Portol.Common.Helper.PaymentMethodType.PayPal });
            return payments;
        }

        public async Task<bool> ResetNewPassword(long mobilePhoned, string newPassword)
        {
            return await _userMobileService.ResetNewPassword(mobilePhoned, newPassword);
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
    }
}
