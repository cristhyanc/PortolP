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
    public class DropIncomingDeliveryViewModel : BaseViewModel
    {
        public ICommand DeliveredCommand { get; private set; }
        
        private List<DeliveryDto> _deliveries;
        public List<DeliveryDto> Deliveries
        {
            get
            {
                return _deliveries;
            }
            set
            {
                _deliveries = value;
                OnPropertyChanged();
            }
        }

        IDeliveryCore _deliveryCore;
        ISessionData _sessionData;

        public DropIncomingDeliveryViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
            _sessionData = sessionData;
            DeliveredCommand = new Command<Guid>(((Guid deliveryid) => MarkAsDelivered(deliveryid)), (Guid deliveryid) => { return !IsBusy; });
            
        }

        private async Task MarkAsDelivered(Guid deliveryID)
        {
            try
            {
                this.IsBusy = true;
                if (await this.UserDialogs.ConfirmAsync(StringResources.ConfirmDelivered, StringResources.Delivery))
                {
                    await _deliveryCore.MarkAsDelivered(deliveryID);
                    Deliveries = await _deliveryCore.GetPendingReceiverDeliveries(_sessionData.User.CustomerID);
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

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                Deliveries = await _deliveryCore.GetPendingReceiverDeliveries(_sessionData.User.CustomerID);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropIncomingDeliveryViewModel", "InitializeAsync");                
            }
            finally
            {
                this.IsBusy = false;
            }
        }

    }
}
