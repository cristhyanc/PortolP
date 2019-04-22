using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class CustomerMobileServiceMK : ICustomerMobileService
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
            customer = customer.GetCustomerByEmail(uow, email);
            Task<CustomerDto> task = Task.Run(() => {
                return Customer.ORM(customer);

            });

            return task;
        }

        public Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {           

            Customer customer = new Customer();
            customer= customer.GetCustomerByPhoneNumber(uow, phoneNumber, countryCode);
            Task<CustomerDto> task = Task.Run(() => {
             return   Customer.ORM(customer);

            });

            return task;
        }
    }
}
