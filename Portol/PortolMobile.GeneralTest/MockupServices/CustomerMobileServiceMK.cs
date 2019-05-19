using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class CustomerMobileServiceMK : IUserMobileService
    {
        IUnitOfWork uow;

        public CustomerMobileServiceMK(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public Task<bool> CreateNewCustomer(CustomerDto newCustomer)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDto> GetCustomerByEmail(string email)
        {
            Customer customer = new Customer();
            customer = Customer.GetCustomerByEmail(uow, email);
            Task<CustomerDto> task = Task.Run(() => {
                return Customer.ORM(customer);

            });

            return task;
        }

        public Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {           

            Customer customer = new Customer();
            customer= Customer.GetCustomerByPhoneNumber(uow, phoneNumber, countryCode);
            Task<CustomerDto> task = Task.Run(() => {
             return   Customer.ORM(customer);

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
