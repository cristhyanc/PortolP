using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
   public interface IDeliveryService
    {
        IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables();
        Guid CreateDeliveryRequest(DeliveryDto dropoff);
    }
}
