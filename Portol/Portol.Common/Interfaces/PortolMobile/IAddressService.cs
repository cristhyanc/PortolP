﻿using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
    public interface IAddressService
    {
        Task<AddressFinderDto> GetPosibleAddresses(AddressDto tentativeAddress);
        Task<AddressFinderDetail> GetAddressMetadata(string addressId);
    }
}