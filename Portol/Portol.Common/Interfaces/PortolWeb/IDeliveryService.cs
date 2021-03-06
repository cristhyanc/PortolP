﻿using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
   public interface IDeliveryService
    {
        IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables();

        Guid CreateDeliveryRequest(DeliveryDto dropoff);

        DriverDto GetDeliveryDriverInfo(Guid deliveryID);

        List<DeliveryDto> GetPendingReceiverDeliveries(Guid receiverID);
        DeliveryDto GetSendertDeliveryInProgress(Guid customerID);
        void ConfirmDeliveryPickUp(Guid deliveryID);
        DeliveryStatus GetDeliveryStatus(Guid deliveryID);
        void RateDelivery(Guid deliveryID, int rate);
        void MarkAsDelivered(Guid deliveryID);

    }
}
