using Portol.Common.DTO;
using Portol.Common.Helper;
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
        Task<DeliveryDto> GetSendertDeliveryInProgress(Guid customerID);
        Task<DriverDto> GetDeliveryDriverInfo(Guid deliveryID);
        Task<DeliveryStatus> GetDeliveryStatus(Guid deliveryID);
        Task<List<DeliveryDto>> GetPendingReceiverDeliveries(Guid receiverID);
        Task<bool> ConfirmDeliveryPickUp(Guid deliveryID);
        Task<bool> RateDelivery(Guid deliveryID, int rate);
        Task<bool> MarkAsDelivered(Guid deliveryID);
        Task<List<DeliveryDto>> GetSentDeliveriesByCustomer(Guid customerId);
    }
}
