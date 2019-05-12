using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms
{
   public interface ISessionData
    {
         CustomerDto User { get; }

        Task LoginUser(ILoginCore userCore, string email, string password);
        string GetCurrentToken();
    }
}
