﻿

using Acr.UserDialogs;
using PortolMobile.Forms.Services.Navigation;

namespace PortolMobile.Forms.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
       
        public MainViewModel( INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
           
          
        }

      
    }
}
