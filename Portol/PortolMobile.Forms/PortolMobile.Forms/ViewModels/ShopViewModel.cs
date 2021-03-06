﻿using Acr.UserDialogs;
using Portol.Common;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
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

        public ShopViewModel( INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            GotoDropOffCommand = new Command(GotoDropOff, () => { return !IsBusy; });
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
