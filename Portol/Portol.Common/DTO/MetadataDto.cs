using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
  public  class MetadataDto
    {
        public string StripeAppSecretKey { get;  set; }
        public string StripeAppPublicKey { get;  set; }
        public string MapAppKey { get; set; }
        public string AddressAppSecretKey { get;  set; }
        public string AddressAppPublicKey { get;  set; }
    }
}
