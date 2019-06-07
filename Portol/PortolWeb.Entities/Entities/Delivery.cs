using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

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
        public DateTime CreatedDate { get; set; }
        public int Rating { get; set; }
        

        public DeliveryStatus DeliveryStatus { get; set; }

        [NotMapped]
        public Driver DriverInformation { get; set; }

        [NotMapped]
        public Customer CustomerSender { get; set; }

        [NotMapped]
        public Customer CustomerReceiver { get; set; }

        [NotMapped]
        public PaymentMethod DeliveryPaymentMethod { get; set; }

        [NotMapped]
        public Address PickupAddress { get; set; }

        [NotMapped]
        public Address DropoffAddress { get; set; }

        [NotMapped]
        public Parcel Parcel { get; set; }

        [NotMapped]
        public List<Picture> Pictures { get; set; }

        
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
            delivery.CreatedDate = deliveryDto.CreatedDate;
            delivery.EstimatedCost = deliveryDto.EstimatedCost;
            deliveryDto.PickupAddress.IsStarterPoint = true;
            delivery.PaymentMethodID = deliveryDto.PaymentMethod.PaymentMethodID;
            delivery.Rating = deliveryDto.Rating;

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

        public  Driver GetDriverInfo(IUnitOfWork uow)
        {
            var delivery = uow.DeliveryRepository.Get(this.DeliveryID);
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

        public static List<Delivery> GetDeliveriesWaitingForDriver(IUnitOfWork uow)
        {
            var result = uow.DeliveryRepository.GetAll(x => x.DeliveryStatus == DeliveryStatus.SearchingDriver);
            return result.ToList();
        }

        public static Delivery GetDeliveryDetails(Guid deliveryID, IUnitOfWork uow)
        {
            var result = uow.DeliveryRepository.Get(deliveryID);
            if (result != null)
            {
                result.DriverInformation = Driver.GetDriverInformation(result.DriverID, uow);
                result.CustomerReceiver = Customer.GetCustomerDetails(uow, result.CustomerReceiverID);
                result.CustomerSender = Customer.GetCustomerDetails(uow, result.CustomerSenderID);
                result.DeliveryPaymentMethod = uow.PaymentMethodRepository.Get(result.PaymentMethodID);
                result.Parcel = uow.ParcelRepository.Get(x => x.ParentID == result.DeliveryID && x.ParentType == ParentType.Delivery);
                result.Pictures = uow.PictureRepository.GetAll(x => x.ParentID == result.DeliveryID && x.ParentType == ParentType.Delivery).ToList();
                result.DropoffAddress = Address.GetDropoffAddress(result.DeliveryID, ParentType.Delivery, uow);
                result.PickupAddress = Address.GetPickUpAddress(result.DeliveryID, ParentType.Delivery, uow);
            }
            return result;
        }

        public static Delivery GetSendertDeliveryInProgress(Guid CustomerID, IUnitOfWork uow)
        {          
            Delivery result = uow.DeliveryRepository.Get(x => x.CustomerSenderID.Equals(CustomerID) && (x.DeliveryStatus == DeliveryStatus.Delivered || x.DeliveryStatus == DeliveryStatus.SearchingDriver  || x.DeliveryStatus == DeliveryStatus.PickingUp || x.DeliveryStatus == DeliveryStatus.InProgress));
            if (result!=null)
            {
                result = Delivery.GetDeliveryDetails(result.DeliveryID, uow); 
            }
            return result;
        }

        public static void ConfirmDeliveryPickUp(Guid deliveryID, IUnitOfWork uow)
        {
            Delivery result = uow.DeliveryRepository.Get(x => x.DeliveryID==deliveryID );
            if (result != null)
            {
                result.DeliveryStatus = DeliveryStatus.InProgress;
                uow.DeliveryRepository.Update(result);
            }
            else
            {
                throw new AppException(StringResources.DeliveryNotFound);
            }
           
        }

        public static void RateDelivery(Guid deliveryID, int rate,  IUnitOfWork uow)
        {
            Delivery result = uow.DeliveryRepository.Get(x => x.DeliveryID == deliveryID);
            if (result != null)
            {
               
                var driver = uow.DriverRepository.Get(result.DriverID);
                if (driver != null)
                {
                    driver.TotalTrips += 1;
                    driver.Rating = (uow.DeliveryRepository.GetAll(x => x.DriverID == result.DriverID && x.DeliveryStatus == DeliveryStatus.Ranked).Sum(x => x.Rating)) + rate;
                    driver.Rating = Math.Round((driver.Rating / driver.TotalTrips), 1);
                    uow.DriverRepository.Update(driver);
                }
                result.Rating = rate;
                result.DeliveryStatus = DeliveryStatus.Ranked;
                uow.DeliveryRepository.Update(result);
            }
            else
            {
                throw new AppException(StringResources.DeliveryNotFound);
            }
        }


        public static void MarkAsDelivered(Guid deliveryID, IUnitOfWork uow)
        {
            Delivery result = uow.DeliveryRepository.Get(x => x.DeliveryID == deliveryID);
            if (result != null)
            {
                result.DeliveryStatus = DeliveryStatus.Delivered ;
                uow.DeliveryRepository.Update(result);
            }
            else
            {
                throw new AppException(StringResources.DeliveryNotFound);
            }
        }

        public static DeliveryStatus GetDeliveryStatus(Guid deliveryID, IUnitOfWork uow)
        {
            Delivery result = uow.DeliveryRepository.Get(x => x.DeliveryID == deliveryID);
            if (result != null)
            {
                return result.DeliveryStatus;
            }
            else
            {
                throw new AppException(StringResources.DeliveryNotFound);
            }
        }

        public static List<Delivery> GetPendingReceiverDeliveries(Guid receiverID, IUnitOfWork uow)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> deliveries = uow.DeliveryRepository.GetAll(x => x.CustomerReceiverID.Equals(receiverID) && x.DeliveryStatus == DeliveryStatus.InProgress ).ToList();
            if (deliveries?.Count > 0)
            {
                foreach (var item in deliveries)
                {
                    var delivery = Delivery.GetDeliveryDetails(item.DeliveryID, uow);
                    if (delivery != null)
                    {
                        result.Add(delivery);
                    }
                }
            }
            return result;
        }


        public DeliveryDto ToDto()
        {
            DeliveryDto delivery = new DeliveryDto();
            delivery.DeliveryID = this.DeliveryID;
            delivery.DeliveryStatus = this.DeliveryStatus;
            delivery.Description = this.Description;
            delivery.CreatedDate = this.CreatedDate;
            delivery.Rating = this.Rating;
            if (this.DropoffAddress!=null)
            {
                delivery.DropoffAddress = this.DropoffAddress.ToDto();
            }
            
            delivery.EstimatedCost = this.EstimatedCost;
            if(Parcel!=null)
            {
                delivery.Parcel = this.Parcel.ToDto();
            }
            
            if(DeliveryPaymentMethod != null)
            {
                delivery.PaymentMethod = this.DeliveryPaymentMethod.ToDto();
            }
           
            if(PickupAddress!=null)
            {
                delivery.PickupAddress = this.PickupAddress.ToDto();
            }
            
            if(Pictures?.Count>0)
            {
                delivery.Pictures = this.Pictures.Select(x=> x.ToDto()).ToList();
            }
            
            if(CustomerReceiver!=null)
            {
                delivery.Receiver = this.CustomerReceiver.ToDto();
            }

            if (CustomerSender != null)
            {
                delivery.Sender = this.CustomerSender.ToDto();
            }
          
            delivery.TravelDistance = this.TravelDistance;
         

            return delivery;

        }
    }
}
