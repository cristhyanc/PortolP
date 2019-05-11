using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class SessionDataMK : ISessionData
    {
        public CustomerDto User { get; private set; }

        public string GetCurrentToken()
        {
            throw new NotImplementedException();
        }

        public async Task LoginUser(ILoginService _loginService, string email, string password)
        {
            User= await _loginService.Authenticate("cristhyan@outlook.com", "");
        }
    }
}
