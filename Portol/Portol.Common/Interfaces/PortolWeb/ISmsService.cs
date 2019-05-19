using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
    public interface ISmsService
    {
        void SendNewCode(long mobileNumber, Int32 countryCode);
    }
}
