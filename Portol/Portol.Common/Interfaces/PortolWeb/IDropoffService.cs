using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
   public interface IDropoffService
    {
        IEnumerable<VehiculeTypeDto> GetVehiculeTypesAvailables();
        Guid CreateDropoffRequest(DropoffDto dropoff);
    }
}
