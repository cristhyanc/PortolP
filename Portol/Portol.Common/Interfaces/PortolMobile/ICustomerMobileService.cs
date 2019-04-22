﻿using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface ICustomerMobileService
    {
        Task<Boolean> CreateNewCustomer(CustomerDto newCustomer);
        Task<CustomerDto> GetCustomerByPhoneNumber(long phoneNumber, int countryCode);
        Task<CustomerDto> GetCustomerByEmail(string email);
    }
}
