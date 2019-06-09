using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortolWeb.Core.DeliveryServices
{
    public class DeliveryService : IDeliveryService
    {
        IUnitOfWork _uow;
        IImageManager _imageManager;
        IPaymentService _paymentService;
        IDeliveryCalculator _deliveryCalculator;
        public DeliveryService(IUnitOfWork uow, IImageManager imageManager, IPaymentService paymentService, IDeliveryCalculator deliveryCalculator  )
        {
            _uow = uow;
            _imageManager = imageManager;
            _paymentService = paymentService;
            _deliveryCalculator = deliveryCalculator;
        }

        public void AssignDriver()
        {
            try
            {
                var deliveries = Delivery.GetDeliveriesWaitingForDriver(_uow).ToList();
                var availableDriver = Driver.GetAvailableDrivers(_uow).ToList();

                if (deliveries?.Count() > 0 && availableDriver?.Count() > 0)
                {
                    foreach (var item in deliveries)
                    {
                        item.DriverID = availableDriver.First().CustomerID;
                        item.DeliveryStatus = DeliveryStatus.PickingUp;
                    }
                    _uow.DeliveryRepository.Update(deliveries);
                    _uow.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DeliveryService.AssignDriver");
            }
        }

        public Guid CreateDeliveryRequest(DeliveryDto delivery)
        {

            Delivery result = Delivery.Create(delivery, _uow);

            delivery.Pictures.ForEach((x) =>
            {
                x.ParentID = result.DeliveryID;
                x.ImageUrl = _imageManager.SaveFile(x.ImageArray, result.DeliveryID.ToString(), x.ImageName, ParentType.Delivery);
                Picture.Create(x, ParentType.Delivery, _uow);

            });


            _uow.SaveChanges();
            return result.DeliveryID;
        }


        public IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables()
        {
            var types = VehiculeType.GetAll(_uow);
            return types.Select(x => x.ToDto());
        }

        public DriverDto GetDeliveryDriverInfo(Guid deliveryID)
        {
            AssignDriver();
            var delivery = new Delivery() { DeliveryID = deliveryID };
            var driver = delivery.GetDriverInfo(_uow);
            if (driver != null)
            {
                return driver.ToDto();
            }
            return null;
        }

        public List<DeliveryDto> GetPendingReceiverDeliveries(Guid receiverID)
        {
            List<DeliveryDto> result = null;
            var deliveries = Delivery.GetPendingReceiverDeliveries(receiverID, _uow);
            if (deliveries?.Count > 0)
            {
                result = deliveries.Select(x => x.ToDto()).ToList();
            }

            return result;
        }

        public List<DeliveryDto> GetSentDeliveriesByCustomer(Guid customerId)
        {
            List<DeliveryDto> result = null;
            var deliveries = Delivery.GetSentDeliveriesByCustomer(customerId, _uow);
            if (deliveries?.Count > 0)
            {
                result = deliveries.Select(x => x.ToDto()).ToList();
            }

            return result;
        }

        
        public DeliveryDto GetSendertDeliveryInProgress(Guid customerID)
        {
          
            var delivery = Delivery.GetSendertDeliveryInProgress(customerID, _uow);
            if (delivery!=null)
            {
                return delivery.ToDto();
            }

            return null;
        }

        public void ConfirmDeliveryPickUp(Guid deliveryID)
        {
            Delivery.ConfirmDeliveryPickUp(deliveryID, _uow);
            _uow.SaveChanges();
        }

        public DeliveryStatus GetDeliveryStatus(Guid deliveryID)
        {
            return Delivery.GetDeliveryStatus(deliveryID, _uow);

        }

        public void RateDelivery(Guid deliveryID, int rate)
        {
            Delivery.RateDelivery(deliveryID, rate, _uow);
            _uow.SaveChanges();
        }

        public async Task MarkAsDelivered(Guid deliveryID)
        {
           
            //calculate value
            var delivery = Delivery.GetDeliveryDetails(deliveryID, _uow).ToDto();
            string paymentid=null;
            if (delivery != null)
            {
                if (delivery.DriverInformation?.CurrentVehicule?.VehiculeType == null)
                {
                    throw new AppException(StringResources.DriverNotFound);
                }

                delivery.TotalCost = await _deliveryCalculator.CalculatePrice(delivery.Parcel, delivery.PickupAddress, delivery.DropoffAddress, delivery.DriverInformation.CurrentVehicule.VehiculeType);
                try
                {
                    var paymentMethod = delivery.Sender.PaymentMethods?.Where(x => x.PaymentMethodID == delivery.PaymentMethod?.PaymentMethodID).FirstOrDefault();
                    paymentid = await _paymentService.ChargeCustomer(delivery.Sender.CustomerPaymentID, paymentMethod?.CardServiceID, delivery.TotalCost);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "DeliveryService.MarkAsDelivered");
                }              

                Delivery.MarkAsDelivered(deliveryID, paymentid, delivery.TotalCost, _uow);
                _uow.SaveChanges();
            }

        }
    }
}
