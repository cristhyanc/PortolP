using Portol.Common;
using PortolMobile.Forms.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels
{
    public class DropViewModel: BaseViewModel
    {
        public ICommand GotoShopCommand { get; private set; }

        public DropViewModel()
        {
            GotoShopCommand = new Command(GotoShop);
        }

        private async void GotoShop()
        {
            try
            {
                await NavigationService.NavigateToAsync<ShopViewModel>();
               
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "GotoShop");
            }
        }
    }
}
