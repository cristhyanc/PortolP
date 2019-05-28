using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
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
        public DeliveryService(IUnitOfWork uow, IImageManager imageManager)
        {
            _uow = uow;
            _imageManager = imageManager;
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
                        item.DeliveryStatus = DeliveryStatus.InProgress;
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
    }
}
