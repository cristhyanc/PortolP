using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
   public class DropoffDto
    {
        public  CustomerDto Receiver { get; set; }
        public CustomerDto Sender { get; set; }
        public AddressDto  PickupAddress { get; set; }
        public AddressDto  DropoffAddress { get; set; }
        public string Description { get; set; }
        public List<PicturesDto> Pictures { get; set; }
        public MeasurementDto Measurements { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }

        
    }
}
