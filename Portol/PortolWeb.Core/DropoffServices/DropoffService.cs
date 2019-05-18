using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Portol.Common.Helper;
using Portol.Common;

namespace PortolWeb.Core.DropoffServices
{
   public class DropoffService: IDropoffService
    {
        private IUnitOfWork _uow;
        public DropoffService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public Guid CreateDropoffRequest(DropoffDto dropoff)
        {

            if (dropoff.Sender == null || dropoff.Receiver == null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            if (dropoff.DropoffAddress == null || dropoff.PickupAddress == null)
            {
                throw new AppException(StringResources.AddressRequired);
            }

            if (!(dropoff.Pictures?.Count>0))
            {
                throw new AppException(StringResources.PictureParcelRequired);
            }

            Dropoff result = Dropoff.Create(dropoff, _uow);
            _uow.SaveChanges();
            return result.DropoffID;
        }

        
        public IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables()
        {
            var types = VehiculeType.GetAll(_uow);
            return types.Select(x => x.ToDto());
        }
    }
}
