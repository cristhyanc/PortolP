using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
  public  class VehiculeTypeDto
    {
        public Guid VehiculeTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingFee { get; set; }
        public decimal CostPerkilometre { get; set; }
        public long MaximumDistance { get; set; }
        public int MaximumWeight { get; set; }
        public int MaximumWidth { get; set; }
        public int MaximumHeight { get; set; }
        public int MaximumLength { get; set; }
              
        public long MaximumVolumne
        {
            get
            {
                return MaximumHeight * MaximumLength * MaximumWidth;
            }
        }
      
        public List<VehiculeTypeRangeDto> Ranges { get; set; }
    }
}
