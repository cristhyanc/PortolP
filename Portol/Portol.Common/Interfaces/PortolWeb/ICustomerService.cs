using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
    public interface ICustomerService
    {
        CustomerDto Authenticate(string username, string password);
        IEnumerable<CustomerDto> GetAll();
        CustomerDto GetById(Guid userId);
        CustomerDto Create(CustomerDto user, string password);
        void Update(CustomerDto user, string password = null);
        void Delete(Guid userId);
        void ResetPassword(CustomerDto user);
        bool VerifyMobileUniqueness(CustomerDto phoneDetails);
        bool VerifyEmailUniqueness(string email);
        bool ValidateVerificationCode(long phoneNumber, Int32 countryCode, Int32 code);
        CustomerDto GetCustomerByPhoneNumber(long phoneNumber, int countryCode);
        CustomerDto GetCustomerByEmail(string email);

    }
}
