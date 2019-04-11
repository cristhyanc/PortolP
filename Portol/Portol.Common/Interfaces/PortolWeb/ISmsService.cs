using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
    public interface ISmsService
    {
        void SendNewCode(string mobileNumber, string countryCode);
    }
}
