using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using Portol.Common.Interfaces.PortolWeb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.DropOff
{
    public class DeliveryCore : IDeliveryCore
    {
        IDeliveryMobileService _deliveryService;
        public DeliveryCore(IDeliveryMobileService deliveryService)
        {
            _deliveryService = deliveryService;
        }
        public async Task<List<VehiculeTypeDto>> GetVehiculeTypesAvailables()
        {
            return await _deliveryService.GetVehiculeTypesAvailables();
        }
        public async Task<List<DeliveryDto>> GetPendingReceiverDeliveries(Guid receiverID)
        {
            return await _deliveryService.GetPendingReceiverDeliveries(receiverID);
        }

        public async Task<List<DeliveryDto>> GetSentDeliveriesByCustomer(Guid customerId)
        {
            return await _deliveryService.GetSentDeliveriesByCustomer(customerId);
        }
        public async Task<Guid> CreateDropoffRequest(DeliveryDto dropoffParcel)
        {
            return await _deliveryService.CreateDeliveryRequest(dropoffParcel);
        }

        public async Task<bool> ConfirmDeliveryPickUp(Guid deliveryID)
        {
            return await _deliveryService.ConfirmDeliveryPickUp(deliveryID);
        }
        public async Task<DriverDto> GetDeliveryDriverInfo(Guid deliveryID)
        {
            return await _deliveryService.GetDeliveryDriverInfo(deliveryID);
        }

        public async Task<DeliveryStatus> GetDeliveryStatus(Guid deliveryID)
        {
            return await _deliveryService.GetDeliveryStatus(deliveryID);
        }
        public async Task<DeliveryDto> GetSendertDeliveryInProgress(Guid customerID)
        {
            return await _deliveryService.GetSendertDeliveryInProgress(customerID);
        }
        public async Task<bool> CancelDelivery(Guid deliveryID)
        {
            return await _deliveryService.CancelDelivery(deliveryID);
        }
        public async Task<bool> RateDelivery(Guid deliveryID, int rate)
        {
            return await _deliveryService.RateDelivery(deliveryID, rate);
        }
        public async Task<bool> MarkAsDelivered(Guid deliveryID)
        {
            return await _deliveryService.MarkAsDelivered(deliveryID);
        }
    }
}
