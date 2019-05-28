using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblPaymentMethod")]
    public class PaymentMethod
    {
        public string CardServiceID { get; set; }
        [Key]
        public Guid PaymentMethodID { get; set; }
        public Guid CustomerID { get; set; }


        public bool Save(PaymentMethodDto paymentMethod, IUnitOfWork uow)
        {
            if (paymentMethod == null)
            {
                return false;
            }

            if(paymentMethod.CustomerID == Guid.Empty )
            {
                throw new AppException(StringResources.CustomerDetailsRequired);
            }

            if(string.IsNullOrEmpty(paymentMethod.CardServiceID))
            {
                throw new AppException(StringResources.CustomerDetailsRequired);
            }

            this.CardServiceID = paymentMethod.CardServiceID;
            this.CustomerID = paymentMethod.CustomerID;
            this.PaymentMethodID = paymentMethod.PaymentMethodID;       
            uow.PaymentMethodRepository.Update(this);
            return true;
        }

        public PaymentMethodDto ToDto()
        {
            PaymentMethodDto methodDto = new PaymentMethodDto();
            methodDto.CustomerID = this.CustomerID;
            methodDto.PaymentMethodID = this.PaymentMethodID;
            methodDto.CardServiceID = this.CardServiceID;
            return methodDto;
        }
    }
}
