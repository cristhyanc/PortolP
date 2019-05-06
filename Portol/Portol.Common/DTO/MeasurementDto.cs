using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
   public class MeasurementDto
    {
        //Centimetre
        public int Width { get; set; }
        //Centimetre
        public int Height { get; set; }
        //Kg
        public int Weight { get; set; }
        //Centimetre
        public int Length { get; set; }

        public int Volume
        {
            get
            {
                return Width * Length * Height;
            }

        }
        
    }
}
