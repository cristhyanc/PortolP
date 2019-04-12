using Portol.Common;
using PortolMobile.Forms.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace PortolMobile.Forms.ViewModels
{
    public class ShopViewModel: BaseViewModel
    {
        public ICommand GotoDropOffCommand { get; private set; }

        public ShopViewModel()
        {
            GotoDropOffCommand = new Command(GotoDropOff);
        }

        private async void GotoDropOff()
        {
            try
            {
                await NavigationService.NavigateToAsync<DropViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "GotoDropOff");
            }
        }
    }
}
