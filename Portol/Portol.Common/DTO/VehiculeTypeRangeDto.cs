using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
  public  class VehiculeTypeRangeDto
    {
        public Guid VehiculeTypeRangeID { get; set; }

        public Guid VehiculeTypeID { get; set; }

        public VehiculeRangeType RangeType { get; set; }
        public decimal MaximumValue { get; set; }
        public decimal MinimumValue { get; set; }
        public decimal CostPerExtraUnit { get; set; }
        public decimal Unit { get; set; }
    }
}
