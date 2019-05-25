using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
   public class DriverDto
    {
        public CustomerDto  Details { get; set; }
        public string DirverLicenceNumber { get; set; }
        public VehiculeDto CurrentVehicule { get; set; }
    }
}
