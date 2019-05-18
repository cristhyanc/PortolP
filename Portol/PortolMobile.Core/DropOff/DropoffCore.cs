using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using Portol.Common.Interfaces.PortolWeb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.DropOff
{
    public class DropoffCore : IDropoffCore
    {
        IDropoffMobileService _dropoffService;
        public DropoffCore(IDropoffMobileService dropoffService)
        {
            _dropoffService = dropoffService;
        }
        public async Task<List<VehiculeTypeDto>> GetVehiculeTypesAvailables()
        {
            return await  _dropoffService.GetVehiculeTypesAvailables();
        }

        public async Task<Guid> CreateDropoffRequest(DropoffDto dropoffParcel)
        {
            return await _dropoffService.CreateDropoffRequest(dropoffParcel);
        }
    }
}
