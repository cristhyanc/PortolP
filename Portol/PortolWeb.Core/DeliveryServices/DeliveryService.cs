using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortolWeb.Core.DeliveryServices
{
    public class DeliveryService: IDeliveryService
    {
         IUnitOfWork _uow;
        IImageManager _imageManager;
        public DeliveryService(IUnitOfWork uow, IImageManager imageManager )
        {
            _uow = uow;
            _imageManager = imageManager;
        }


        public Guid CreateDeliveryRequest(DeliveryDto delivery)
        {

            if (delivery.Sender == null || delivery.Receiver == null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            if (delivery.DropoffAddress == null || delivery.PickupAddress == null)
            {
                throw new AppException(StringResources.AddressRequired);
            }

            if (!(delivery.Pictures?.Count>0))
            {
                throw new AppException(StringResources.PictureParcelRequired);
            }

            Delivery result = Delivery.Create(delivery, _uow);

            delivery.Pictures.ForEach((x) =>
            {
                x.ParentID = result.DeliveryID;
                x.ImageUrl = _imageManager.SaveFile(x.ImageArray, result.DeliveryID.ToString(), x.ImageName, ParentType.Delivery);
                Picture.Create(x, ParentType.Delivery , _uow);

            });

            _uow.SaveChanges();
            return result.DeliveryID;
        }

        
        public IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables()
        {
            var types = VehiculeType.GetAll(_uow);
            return types.Select(x => x.ToDto());
        }
    }
}
