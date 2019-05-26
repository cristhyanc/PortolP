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
    [Table("tblDelivery")]
    public class Delivery
    {
        [Key]
        public Guid DeliveryID { get; set; }
        public Guid CustomerSenderID { get; set; }
        public Guid CustomerReceiverID { get; set; }
        public Guid PaymentMethodID { get; set; }
        public Guid DriverID { get; set; }
        public string Description { get; set; }
        public decimal TravelDistance { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal TotalCost { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }

        [NotMapped]
        public Driver Driver { get; set; }

        public static Delivery Create(DeliveryDto deliveryDto, IUnitOfWork uow)
        {
            if (deliveryDto.Sender == null || deliveryDto.Receiver == null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            if (deliveryDto.DropoffAddress == null || deliveryDto.PickupAddress == null)
            {
                throw new AppException(StringResources.AddressRequired);
            }

            if (!(deliveryDto.Pictures?.Count > 0))
            {
                throw new AppException(StringResources.PictureParcelRequired);
            }

            Delivery delivery = new Delivery();
            delivery.CustomerReceiverID = deliveryDto.Receiver.CustomerID;
            delivery.CustomerSenderID = deliveryDto.Sender.CustomerID;
            delivery.DeliveryStatus = DeliveryStatus.SearchingDriver;
            delivery.Description = deliveryDto.Description;
            delivery.EstimatedCost = deliveryDto.EstimatedCost;
            deliveryDto.PickupAddress.IsStarterPoint = true;
            delivery.PaymentMethodID = deliveryDto.PaymentMethod.PaymentMethodID;

            var payment = uow.PaymentMethodRepository.Get(x => x.CardServiceID.Equals(deliveryDto.PaymentMethod.CardServiceID));

            if (payment != null)
            {
                delivery.PaymentMethodID = payment.PaymentMethodID;
            }

            uow.DeliveryRepository.Insert(delivery);

            deliveryDto.Parcel.ParentID = delivery.DeliveryID;
            deliveryDto.PickupAddress.ParentID = delivery.DeliveryID;
            deliveryDto.DropoffAddress.ParentID = delivery.DeliveryID;

            Address.Create(deliveryDto.PickupAddress, ParentType.Delivery, uow);
            Address.Create(deliveryDto.DropoffAddress, ParentType.Delivery, uow);
            Parcel.Create(deliveryDto.Parcel, ParentType.Delivery, uow);

            return delivery;

        }
        
        public static Driver GetDriverInfo(Guid deliveryID, IUnitOfWork uow)
        {
            var delivery = uow.DeliveryRepository.Get(deliveryID);
            if (delivery == null)
            {
                throw new AppException(StringResources.NoDeliveryInfo);
            }

            Driver result = null;
            if (delivery.DriverID != Guid.Empty)
            {
                result = Driver.GetDriverInformation(delivery.DriverID, uow);
            }
            return result;
        }

        public static IEnumerable <Delivery> GetDeliveriesWaitingForDriver(IUnitOfWork uow)
        {
            var result = uow.DeliveryRepository.GetAll(x => x.DeliveryStatus == DeliveryStatus.SearchingDriver);
            return result;
        }

    }
}
