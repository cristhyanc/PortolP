using Acr.UserDialogs;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
  public  class DropPicturesViewModel: BaseViewModel
    {
        INavigationService navigationService;
        IUserDialogs userDialogs;
        public ICommand AddressEntryCommand { get; private set; }



        public DropPicturesViewModel(INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
            navigationService = _navigationService;
            userDialogs = _userDialogs;
         //   AddressEntryCommand = new Command<string>(((x) => GotoAddressPage(x)));
          
        }
    }
}
