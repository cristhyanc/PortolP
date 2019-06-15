using Portol.Common.DTO;
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

        public async  Task<string> ChargeCustomer(string customerId, string paymentMethodId, decimal amount)
        {
            try
            {
                //Australia so far
                long serviceAmount = (long)(Math.Round(amount,2) * 100);
                var options = new ChargeCreateOptions
                {
                    Amount = serviceAmount,
                    Currency = "aud",
                    Description = "PortolApp Service",
                    CustomerId = customerId                    
                      
                };
                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);
                return charge.Id;
            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }
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

        public async Task<string> LinkNewCreditCard(string customerServiceID, PaymentMethodDto paymentMethod)
        {
            try
            {
                var tokenOptions = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = paymentMethod.CardNumber.Replace(" ", ""),
                        ExpYear = paymentMethod.ExpYear ,
                        ExpMonth = paymentMethod.ExpMonth,
                        Cvc = paymentMethod.CVV 
                    }
                };

                var tokenService = new TokenService();
                Token stripeToken = await tokenService.CreateAsync(tokenOptions);

                var carOptions = new CardCreateOptions
                {
                    SourceToken = stripeToken.Id
                };

                var carService = new CardService();
                var card = await carService.CreateAsync(customerServiceID, carOptions);               
                return card.Id;
            }
            catch (Exception ex)
            {
                throw HandleStripeExceptions(ex);
            }
        }

        public async Task<bool> DeleteCreditCard(string customerServiceID, string creditcardId)
        {
            try
            { 
                var carService = new CardService();
                var card = await carService.DeleteAsync(customerServiceID, creditcardId);
                return card.Deleted.Value ;
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

                var service = new CardService();
                var cards = await service.ListAsync(customerServiceID, null);                
                if (cards != null && cards.Data?.Count>0 )
                {
                    cards.Data.ForEach((x) =>
                    {
                        result.Add(
                            new PaymentMethodDto
                            {
                                CardNumber = x.Last4,
                                ExpMonth = (int)x.ExpMonth,
                                ExpYear = (int)x.ExpYear,
                                CreditCardType = GetMethodType(x.Brand),
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
            switch (brand.ToLower())
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
                        throw new AppException(e.StripeError.Message); 
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
