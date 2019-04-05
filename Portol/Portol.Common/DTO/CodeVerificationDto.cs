using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
  public  class CodeVerificationDto
    {
        public Guid CodeID { get; set; }
        public Int32 CodeNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public DateTime Created { get; set; }
    }
}
