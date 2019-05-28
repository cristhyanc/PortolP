using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
   public class DeliveryDto
    {
        public Guid DeliveryID { get; set; }
        public  CustomerDto Receiver { get; set; }
        public CustomerDto Sender { get; set; }
        public AddressDto  PickupAddress { get; set; }
        public AddressDto  DropoffAddress { get; set; }
        public string Description { get; set; }
        public List<PictureDto> Pictures { get; set; }
        public ParcelDto Parcel { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public decimal TravelDistance { get; set; }
        public decimal EstimatedCost { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        

    }
}
