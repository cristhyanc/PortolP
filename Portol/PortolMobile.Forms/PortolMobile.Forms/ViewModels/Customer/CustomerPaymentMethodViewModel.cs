using Acr.UserDialogs;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolMobile.Forms.ViewModels.Customer
{
   public class CustomerPaymentMethodViewModel: BaseViewModel
    {
        public CustomerPaymentMethodViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
           
        }
    }
}
