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

        public string StripeAppSecretKey { get; private set; }
        public string StripeAppPublicKey { get; private set; }
        public string MapAppKey { get; private set; }
        public string AddressAppSecretKey { get; private set; }
        public string AddressAppPublicKey { get; private set; }

        public async Task RefreshUserDetails(IUserCore userCore)
        {
            var result = await userCore.GetCustomer(User.CustomerID);
            result.Token = this.User.Token;
            User = result;
        }

        public void LogoutUser( )
        {
            if (Application.Current.Properties.ContainsKey("email") )
            {
                Application.Current.Properties.Remove("email");
            }

            if ( Application.Current.Properties.ContainsKey("password"))
            {
                Application.Current.Properties.Remove("password");
            }
            User = null;
        }

        public async Task LoginUser(ILoginCore loginCore, string email, string password)
        {
            var result = await loginCore.Authenticate(email, password);
            User = result;

            Application.Current.Properties["email"] = email;
            Application.Current.Properties["password"] = password;
            Application.Current.SavePropertiesAsync();
        }


        public async Task GetMetadata(ILoginCore loginCore)
        {
            var result = await loginCore.GetMetadata("AjPaETRxkyP3rSDJ7vu2nce9mlY66bgZu0DvY_eIVpeSM5PES53q_9IGzOrxahcL");
            AddressAppPublicKey = result?.AddressAppPublicKey;
            AddressAppSecretKey = result?.AddressAppSecretKey;
            MapAppKey = result?.MapAppKey;
            StripeAppPublicKey = result?.StripeAppPublicKey;
            StripeAppSecretKey = result?.StripeAppSecretKey;
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
