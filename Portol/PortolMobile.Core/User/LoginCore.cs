using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.User
{
    public class LoginCore: ILoginCore
    {

        ILoginService _loginService;
        

        public LoginCore(ILoginService loginService)
        {
            
            _loginService = loginService;
        }

      
       

        public async Task<CustomerDto> Authenticate(string email, string password)
        {
            return await _loginService.Authenticate(email, password);
        }

      

    }
}
