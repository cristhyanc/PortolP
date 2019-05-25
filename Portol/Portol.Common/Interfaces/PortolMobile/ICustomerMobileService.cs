using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface IUserMobileService
    {
        Task<Boolean> SavePaymentMethod(PaymentMethodDto paymentMethod);
        Task<Boolean> SaveCustomer(CustomerDto user);
        Task<Boolean> CreateNewCustomer(CustomerDto newCustomer);
        Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode);
        Task<CustomerDto> GetCustomerByEmail(string email);
        Task<Boolean> VerifyMobileUniqueness(long mobilePhoned, Int32 code);
        Task<Boolean> VerifyEmailUniqueness(string email);
        Task<bool> SendVerificationCode(long mobilePhoned, Int32 code);
        Task<bool> VerifyCode(long mobilePhoned, Int32 countryCode, Int32 code);
        Task<bool> ResetNewPassword(long mobilePhoned, string newPassword);
    }
}
