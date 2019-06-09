using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolWeb
{
    public interface ISmsService
    {
        Task SendNewCode(long mobileNumber, Int32 countryCode);
        Task SendMessage(long mobileNumber, Int32 countryCode, string fullName, string message);
    }
}
