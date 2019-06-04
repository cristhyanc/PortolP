using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

            Application.Current.Properties["email"] = email;
            Application.Current.Properties["password"] = password;
            Application.Current.SavePropertiesAsync();
        }

        public async Task<bool> AutoLoginLastUser(ILoginCore loginCore)
        {
            if (Application.Current.Properties.ContainsKey("email") && Application.Current.Properties.ContainsKey("password"))
            {
                var result = await loginCore.Authenticate(Application.Current.Properties["email"].ToString(), Application.Current.Properties["password"].ToString());
                User = result;
                return true;
            }
            return false;
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
