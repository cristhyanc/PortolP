using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropHistoryDeliveriesViewModel: BaseViewModel
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

        //   private DeliveryDto _selectedDelivery;
        public DeliveryDto SelectedDelivery
        {
            get
            {
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.NavigationService.NavigateToAsync<DropIncomingDeliveryViewModel>(value);
                }

              //  OnPropertyChanged();
            }
        }

        IDeliveryCore _deliveryCore;
        ISessionData _sessionData;

        public DropHistoryDeliveriesViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
            _sessionData = sessionData;

        }

        protected override async void PageAppearing()
        {
            try
            {
                this.IsBusy = true;
                Deliveries = await _deliveryCore.GetSentDeliveriesByCustomer(_sessionData.User.CustomerID);               
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropIncomingDeliveriesViewModel", "PageAppearing");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


    }
}
