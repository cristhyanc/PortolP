using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms
{
   public class SessionData
    {
        static public CustomerDto User { get; private set; }

        static public async Task LoginUser(ILoginService _loginService, string email, string password)
        {
            var result = await _loginService.Authenticate(email, password);
            User = result;
        }
    }
}
