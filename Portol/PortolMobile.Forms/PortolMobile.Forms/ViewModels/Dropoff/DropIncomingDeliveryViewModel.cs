using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropIncomingDeliveryViewModel : BaseViewModel
    {

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
