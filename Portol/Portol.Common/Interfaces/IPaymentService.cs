using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces
{
   public interface IPaymentService
    {
      
        void GetCustomers();
        Task<string> CreateCustomer(CustomerDto customer);
        Task<string> LinkNewCreditCard(string customerServiceID, PaymentMethodDto paymentMethod);
        Task<List<PaymentMethodDto>> GetCustomerPaymentMethods(string customerServiceID);
        Task<string> ChargeCustomer(string customerId, string paymentMethodId, decimal amount);
    }
}
