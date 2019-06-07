using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
  public  class CodeVerificationDto
    {
        public Guid CodeID { get; set; }
        public Int32 CodeNumber { get; set; }
        public long PhoneNumber { get; set; }
        public Int32 CountryCode { get; set; }
        public DateTime Created { get; set; }
    }
}
