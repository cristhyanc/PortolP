using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class PaymentMethodDto
    {
        public Guid PaymentMethodID { get; set; }
        public Guid CustomerID { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string Country { get; set; }
        public bool CurrentCard { get; set; }
        public PaymentMethodType CreditCardType { get; set; }

        public string LastNumbers
        {
            get
            {
                if (!string.IsNullOrEmpty(CardNumber))
                {
                    if (CardNumber.Length > 3)
                    {
                        return CardNumber.Substring((CardNumber.Length - 5), 4);
                    }
                    return CardNumber;
                }
                return "";
            }
        }

        public string IconResourcePath
        {
            get
            {
                return HelperClass.GetCreditCardResourceFile(CreditCardType);

            }
        }

        public string IconName
        {
            get
            {
                return HelperClass.GetCreditCardRFileName(CreditCardType);

            }
        }

    }
}
