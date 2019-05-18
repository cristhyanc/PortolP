using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces
{
  public  interface IDropoffCalculator
    {
        Task<decimal> EstimatePrice(MeasurementDto measurement , AddressDto pickup, AddressDto dropoff, List<VehiculeTypeDto> vehiculeTypes);
    }
}
