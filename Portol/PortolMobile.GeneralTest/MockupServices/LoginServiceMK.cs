using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class LoginServiceMK : ILoginService
    {
        IUnitOfWork uow;
        public LoginServiceMK(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public Task<CustomerDto> Authenticate(string email, string password)
        {
            Customer customer  = Customer.GetCustomerByEmail(uow, email);
            Task<CustomerDto> task = Task.Run(() => {
                return customer.ToDto();

            });

            return task;
        }

        public Task<bool> ResetNewPassword(long mobilePhoned, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendVerificationCode(long mobilePhoned, int code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyCode(long mobilePhoned, int countryCode, int code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyEmailUniqueness(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyMobileUniqueness(long mobilePhoned, int code)
        {
            throw new NotImplementedException();
        }
    }
}
