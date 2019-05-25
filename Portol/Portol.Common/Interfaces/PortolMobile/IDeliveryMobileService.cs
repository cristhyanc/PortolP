using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface IDeliveryMobileService
    {
        Task<List<VehiculeTypeDto>> GetVehiculeTypesAvailables();
        Task<Guid> CreateDeliveryRequest(DeliveryDto delivery);
        Task<DriverDto> GetDeliveryDriverInfo(Guid deliveryID);
    }
}
