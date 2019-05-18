using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblDropoff")]
    public class Dropoff
    {
        [Key]
        public Guid DropoffID { get; set; }
        public Guid CustomerSenderID { get; set; }
        public Guid CustomerReceiverID { get; set; }
        public Guid PaymentMethodID { get; set; }
        public string Description { get; set; }
        public double TravelDistance { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal TotalCost { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }


        public static Dropoff Create(DropoffDto dropoffDto, IUnitOfWork uow)
        {

            Dropoff dropoff = new Dropoff();
            dropoff.CustomerReceiverID = dropoffDto.Receiver.CustomerID;
            dropoff.CustomerSenderID = dropoffDto.Sender.CustomerID;
            dropoff.DeliveryStatus = DeliveryStatus.SearchingDriver;
            dropoff.Description = dropoffDto.Description;
            dropoff.EstimatedCost = dropoffDto.EstimatedCost;
            dropoff.Height = dropoffDto.Measurements.Height;
            dropoff.Length = dropoffDto.Measurements.Length;
            dropoff.PaymentMethodID = dropoffDto.PaymentMethod.PaymentMethodID;
            dropoff.Weight = dropoffDto.Measurements.Weight;
            dropoff.Width = dropoffDto.Measurements.Width;
            dropoffDto.PickupAddress.IsStarterPoint = true;


            uow.DropoffRangeRepository.Insert(dropoff);

            dropoffDto.PickupAddress.ParentID = dropoff.DropoffID;
            dropoffDto.DropoffAddress.ParentID = dropoff.DropoffID;

            Address.Create(dropoffDto.PickupAddress, ParentType.Dropoff, uow);
            Address.Create(dropoffDto.DropoffAddress, ParentType.Dropoff, uow);

            dropoffDto.Pictures.ForEach((x) =>
            {
                x.ParentID = dropoff.DropoffID;
                Picture.Create(x, uow);

            });

            return dropoff;

        }

    }
}
