using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms
{
    public class SessionData : ISessionData
    {
        public CustomerDto User { get; private set; }


        public async Task RefreshUserDetails(IUserCore userCore)
        {
            var result = await userCore.GetCustomer(User.CustomerID);
            result.Token = this.User.Token;
            User = result;
        }

        public async Task LoginUser(ILoginCore loginCore, string email, string password)
        {
            var result = await loginCore.Authenticate(email, password);
            User = result;
        }

        public string GetCurrentToken()
        {
            if (User != null && !string.IsNullOrEmpty(User.Token))
            {
                return User.Token;
            }
            else
            {
                return "";
            }
        }

    }
}
