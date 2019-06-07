using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces
{
   public interface IMapService
    {
        Task<double> CalculateDistance(AddressDto pickup, AddressDto dropoff);
    }
}
