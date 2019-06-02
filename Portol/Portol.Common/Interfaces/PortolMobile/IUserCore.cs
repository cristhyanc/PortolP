using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface IUserCore
    {
        Task<Boolean> SavePaymentMethod(PaymentMethodDto paymentMethod);
        Task<Boolean> SaveCustomer(CustomerDto user);
        Task<Boolean> CreateNewCustomer(CustomerDto newUser);
        Task<CustomerDto> GetCustomerByEmail(string email);
        Task<CustomerDto> GetCustomer(Guid customerID);
        Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode);
        Task<Boolean> VerifyMobileUniqueness(long mobilePhoned, Int32 code);
        Task<Boolean> VerifyEmailUniqueness(string email);
        Task<bool> ResetNewPassword(long mobilePhoned, string newPassword);
        Task<bool> VerifyCode(long mobilePhoned, Int32 countryCode, Int32 code);
        Task<bool> SendVerificationCode(long mobilePhoned, Int32 code);

    }
}
