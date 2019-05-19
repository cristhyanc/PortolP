using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces
{
  public  interface IDeliveryCalculator
    {
        Task<decimal> EstimatePrice(ParcelDto measurement , AddressDto pickup, AddressDto dropoff, List<VehiculeTypeDto> vehiculeTypes);
    }
}
