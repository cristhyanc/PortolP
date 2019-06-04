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

        Task LoginUser(ILoginCore loginCore, string email, string password);
        Task RefreshUserDetails(IUserCore userCore);
        string GetCurrentToken();
        Task<bool> AutoLoginLastUser(ILoginCore loginCore);
    }
}
