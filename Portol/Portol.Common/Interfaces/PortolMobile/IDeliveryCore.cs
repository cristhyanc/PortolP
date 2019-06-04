using Portol.Common.DTO;
using Portol.Common.Helper;
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
        Task<DeliveryStatus> GetDeliveryStatus(Guid deliveryID);
        Task<DeliveryDto> GetSendertDeliveryInProgress(Guid customerID);
        Task<bool> ConfirmDeliveryPickUp(Guid deliveryID);
        Task<List<DeliveryDto>> GetPendingReceiverDeliveries(Guid receiverID);
        Task<bool> RateDelivery(Guid deliveryID, int rate);
    }
}
