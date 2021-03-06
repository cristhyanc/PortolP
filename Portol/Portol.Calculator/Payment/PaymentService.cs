﻿using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Calculator.Payment
{
    public class PaymentService : IPaymentService
    {
        string secret_key;
        string public_key;
        public PaymentService(string secretKey, string publicKey)
        {
            secret_key = secretKey;
            publicKey = public_key;
            StripeConfiguration.SetApiKey(secretKey);
        }


        public async Task<string> CreateCustomer(CustomerDto customer)
        {
            try
            {
                //AddressOptions address = new AddressOptions();
                //address.
                var options = new CustomerCreateOptions
                {
                    Description = "Customer for Portol",
                    Name = customer.FullName,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber.ToString()
                };
                var service = new CustomerService();
                Customer newCustomer = await service.CreateAsync(options);
                return newCustomer.Id;
             
            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }   
        }

        public async Task<string > LinkNewCreditCard(string customerServiceID, PaymentMethodDto paymentMethod)
        {
            try
            {               
                var options = new PaymentMethodCreateOptions
                {        
                    Type="card",
                    Card = new PaymentMethodCardCreateOptions
                    {
                        Cvc = paymentMethod.CVV,
                        ExpMonth = paymentMethod.ExpMonth,
                        ExpYear = paymentMethod.ExpYear,
                        Number = paymentMethod.CardNumber
                    }
                };

                var service = new PaymentMethodService();
                var newPaymentMethod = await service.CreateAsync(options);

                var optionsAttach = new PaymentMethodAttachOptions
                {
                     CustomerId= customerServiceID
                };

                 var response = await service.AttachAsync(newPaymentMethod.Id, optionsAttach);
                return response.Id;

            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }
        }

        public async Task<List<PaymentMethodDto>> GetCustomerPaymentMethods(string customerServiceID)
        {
            try
            {
                if(string.IsNullOrEmpty(customerServiceID))
                {
                    return null;
                }

                List<PaymentMethodDto> result = new List<PaymentMethodDto>();

                var service = new PaymentMethodService();
                var options = new PaymentMethodListOptions
                {
                     CustomerId= customerServiceID,
                     Type="card"
                };

                var payments = await service.ListAsync(options);

                if(payments!=null && payments.Data?.Count>0 )
                {
                    payments.Data.ForEach((x) =>
                    {
                        result.Add(
                            new PaymentMethodDto
                            {
                                CardNumber = x.Card.Last4,
                                ExpMonth = (int)x.Card.ExpMonth,
                                ExpYear = (int)x.Card.ExpYear,
                                CreditCardType = GetMethodType(x.Card.Brand ),
                                 CardServiceID=x.Id
                            });
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }
        }

        private PaymentMethodType GetMethodType (string brand)
{
            switch (brand)
            {
                case "visa":
                return PaymentMethodType.Visa;
                case "mastercard":
                    return PaymentMethodType.MasterCard ;
                default:
                    return PaymentMethodType.None;
            }
        }

        public async void GetCustomers()
        {
            try
            {               

                var service1 = new CustomerService();
                var options1 = new CustomerListOptions
                {
                    Limit = 3,
                };
                var customers = await service1.ListAsync(options1);
            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }
        }

        private Exception HandleStripeExceptions(Exception ex)
        {
            if (ex is StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        Console.WriteLine("Code: " + e.StripeError.Code);
                        Console.WriteLine("Message: " + e.StripeError.Message);
                        break;
                    case "api_connection_error":
                        break;
                    case "api_error":
                        break;
                    case "authentication_error":
                        break;
                    case "invalid_request_error":
                        if(e.StripeError.Code.Equals("incorrect_number"))
                        {
                            throw new AppException(e.StripeError.Message);
                        }
                        break;
                    case "rate_limit_error":
                        break;
                    case "validation_error":
                        break;
                   
                    default:
                        // Unknown Error Type
                        break;
                }

                return ex;
            }
            else
            {
                return ex;
            }
        }
    }
}
