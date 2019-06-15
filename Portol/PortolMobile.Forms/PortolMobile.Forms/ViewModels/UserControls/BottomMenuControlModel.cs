using Acr.UserDialogs;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Customer;
using PortolMobile.Forms.ViewModels.Dropoff;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.UserControls
{
    public class BottomMenuControlModel : BaseViewModel
    {
        public ICommand GoToDeliveriesCommand { get; private set; }
        public ICommand GoToAccountCommand { get; private set; }


        string _bellIcon = "resource://PortolMobile.Forms.Resources.ic_bell.svg?assembly=PortolMobile.Forms";
        public string BellIcon
        {
            get { return _bellIcon; }
            set
            {
                _bellIcon = value;
                OnPropertyChanged();
            }
        }

        ISessionData _sessionData;
        IDeliveryCore _deliveryCore;

        public BottomMenuControlModel(INavigationService navigationService, IUserDialogs userDialogs, IDeliveryCore deliveryCore, ISessionData sessionData) : base(navigationService, userDialogs)
        {
            GoToAccountCommand = new Command((() => GoToAccount()), () => { return !IsBusy; });
            GoToDeliveriesCommand = new Command((() => GoToIncomingDeliveries()), () => { return !IsBusy; });
            _deliveryCore = deliveryCore;
            _sessionData = sessionData;
            _deliveryCore = deliveryCore;
        }

        public async Task InitControl()
        {
            try
            {
               
                BellIcon = "resource://PortolMobile.Forms.Resources.ic_bell.svg?assembly=PortolMobile.Forms";
                var incoming = await _deliveryCore.GetPendingReceiverDeliveries(_sessionData.User.CustomerID);
                if (incoming?.Count > 0)
                {
                    BellIcon = "resource://PortolMobile.Forms.Resources.ic_bellRed.svg?assembly=PortolMobile.Forms";
                }              
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "BottomMenuControlModel", "PageAppearing");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


        private async void GoToAccount()
        {
            try
            {
                await NavigationService.NavigateToAsync<CustomerAccountViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "GoToAccount");
            }
        }

        private async void GoToIncomingDeliveries()
        {
            try
            {
                await NavigationService.NavigateToAsync<DropIncomingDeliveriesViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "GoToIncomingDeliveries");
            }
        }
    }
}
