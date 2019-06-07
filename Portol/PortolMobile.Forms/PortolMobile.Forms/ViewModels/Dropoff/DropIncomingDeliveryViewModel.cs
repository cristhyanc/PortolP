using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropIncomingDeliveryViewModel: BaseViewModel
    {
        public ICommand DeliveredCommand { get; private set; }

        IDeliveryCore _deliveryCore;
        ISessionData _sessionData;
       
        private DeliveryDto _delivery;
        public DeliveryDto Delivery
        {
            get
            {
                return _delivery;
            }
            set
            {
                _delivery = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePicture
        {
            get { return _sessionData?.User?.ProfilePhoto?.ImageUrl; }

        }


        public DropIncomingDeliveryViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
            _sessionData = sessionData;
            DeliveredCommand = new Command((( ) => MarkAsDelivered()), ( ) => { return !IsBusy; });

        }
        
        public override  Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                Delivery = (DeliveryDto)navigationData;
                
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropIncomingDeliveryViewModel", "InitializeAsync");
            }
            finally
            {
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task MarkAsDelivered( )
        {
            try
            {
                this.IsBusy = true;
                if (await this.UserDialogs.ConfirmAsync(StringResources.ConfirmDelivered, StringResources.Delivery))
                {
                    await _deliveryCore.MarkAsDelivered(Delivery.DeliveryID);
                    await this.NavigationService.GoToPreviousPageAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropIncomingDeliveryViewModel", "MarkDelivered");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


    }
}
