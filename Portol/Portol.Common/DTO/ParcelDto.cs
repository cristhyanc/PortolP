using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
   public class ParcelDto
    {
        public ParcelDto()
        {
            ParcelID = Guid.NewGuid();
        }
        public Guid ParcelID { get; set; }
        public Guid ParentID { get; set; }
       // public int ParentType { get; set; }
        //Centimetre
        public int Width { get; set; }
        //Centimetre
        public int Height { get; set; }
        //Kg
        public int Weight { get; set; }
        //Centimetre
        public int Length { get; set; }

        public decimal Worth { get; set; }

        public int Volume
        {
            get
            {
                return Width * Length * Height;
            }

        }
        
    }
}
