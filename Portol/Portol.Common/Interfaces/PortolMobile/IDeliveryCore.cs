using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
    public interface IDeliveryCore
    {
        Task<List<VehiculeTypeDto>> GetVehiculeTypesAvailables();
        Task<Guid> CreateDropoffRequest(DeliveryDto dropoffParcel);
        Task<DriverDto> GetDeliveryDriverInfo(Guid deliveryID);
        Task<List<DeliveryDto>> GetPendingReceiverDeliveries(Guid receiverID);
    }
}
