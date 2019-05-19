using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblDelivery")]
    public class Delivery
    {
        [Key]
        public Guid DeliveryID { get; set; }
        public Guid CustomerSenderID { get; set; }
        public Guid CustomerReceiverID { get; set; }
        public Guid PaymentMethodID { get; set; }
        public string Description { get; set; }
        public double TravelDistance { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal TotalCost { get; set; }
       
        public DeliveryStatus DeliveryStatus { get; set; }


        public static Delivery Create(DeliveryDto deliveryDto, IUnitOfWork uow)
        {

            Delivery delivery = new Delivery();
            delivery.CustomerReceiverID = deliveryDto.Receiver.CustomerID;
            delivery.CustomerSenderID = deliveryDto.Sender.CustomerID;
            delivery.DeliveryStatus = DeliveryStatus.SearchingDriver;
            delivery.Description = deliveryDto.Description;
            delivery.EstimatedCost = deliveryDto.EstimatedCost;
            delivery.PaymentMethodID = deliveryDto.PaymentMethod.PaymentMethodID;           
            deliveryDto.PickupAddress.IsStarterPoint = true;

            uow.DeliveryRepository.Insert(delivery);

            deliveryDto.Parcel.ParentID = delivery.DeliveryID;
            deliveryDto.PickupAddress.ParentID = delivery.DeliveryID;
            deliveryDto.DropoffAddress.ParentID = delivery.DeliveryID;

            Address.Create(deliveryDto.PickupAddress, ParentType.Delivery, uow);
            Address.Create(deliveryDto.DropoffAddress, ParentType.Delivery, uow);
            Parcel.Create(deliveryDto.Parcel , ParentType.Delivery, uow);

            return delivery;

        }

    }
}
