using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortolWeb.API.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string ServerUrl { get; set; }
        public string DBScriptPath { get; set; }
        public string LogPaht { get; set; }
        public string SinchAppKey { get; set; }
        public string SinchAppSecret { get; set; }
        public string StripeAppSecretKey { get; set; }
        public string StripeAppPublicKey { get; set; }
        public string MapAppKey { get; set; }
        public string AddressAppSecretKey { get; set; }
        public string AddressAppPublicKey { get; set; }

    }
}
