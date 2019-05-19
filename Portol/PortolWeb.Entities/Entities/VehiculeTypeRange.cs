using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{

    [Table("tblVehiculeTypeRange")]
    public  class VehiculeTypeRange
    {
        [Key]
        public Guid VehiculeTypeRangeID { get; set; }
      
        public Guid VehiculeTypeID { get; set; }

        public VehiculeRangeType RangeType { get; set; }
        public decimal MaximumValue { get; set; }
        public decimal MinimumValue { get; set; }
        public decimal CostPerExtraUnit { get; set; }
        public decimal Unit { get; set; }

        public VehiculeTypeRangeDto ToDto()
        {
            VehiculeTypeRangeDto result = new VehiculeTypeRangeDto();
            result.VehiculeTypeRangeID = this.VehiculeTypeRangeID;
            result.VehiculeTypeID = this.VehiculeTypeID;
            result.RangeType = this.RangeType;
            result.MaximumValue = this.MaximumValue;
            result.MinimumValue = this.MinimumValue;
            result.CostPerExtraUnit = this.CostPerExtraUnit;
            result.Unit = this.Unit;
            return  result;
        }
    }
}
