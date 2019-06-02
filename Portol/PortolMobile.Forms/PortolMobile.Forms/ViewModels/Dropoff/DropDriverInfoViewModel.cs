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
   public class DropDriverInfoViewModel: BaseViewModel
    {


        string _deliveryStatusMessage=StringResources.DriverOnWay;
        public string DeliveryStatusMessage
        {
            get
            {
                return _deliveryStatusMessage;
            }
            set
            {
                _deliveryStatusMessage = value;
                OnPropertyChanged();
            }

        }

        bool _isPickedupButtonVisible;
        public bool IsPickedupButtonVisible
        {
            get
            {
                return _isPickedupButtonVisible;
            }
            set
            {
                _isPickedupButtonVisible = value;
                OnPropertyChanged();
            }

        }

        bool _isSearchingDriver;
        public bool IsSearchingDriver
        {
            get
            {
                return _isSearchingDriver;
            }
            set
            {
                _isSearchingDriver = value;
                OnPropertyChanged();
            }

        }

        bool _isRatingVisible;
        public bool IsRatingVisible
        {
            get
            {
                return _isRatingVisible;
            }
            set
            {
                _isRatingVisible = value;
                OnPropertyChanged();
            }

        }

        int _driverRating;
        public int DriverRating
        {
            get
            {
                return _driverRating;
            }
            set
            {
                _driverRating = value;
                OnPropertyChanged();
            }

        }

        DriverDto _driverInfo;
        public DriverDto DriverInfo
        {
            get
            {
                return _driverInfo;
            }
            set
            {
                _driverInfo = value;
                OnPropertyChanged();
            }

        }
        bool _stopDeliveryStatusLoop;
        public ICommand ConfirmPickupCommand { get; private set; }
        DeliveryDto delivery;
        IDeliveryCore _deliveryCore;
        public DropDriverInfoViewModel(INavigationService navigationService, IUserDialogs userDialogs, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
            ConfirmPickupCommand = new Command((() => ConfirmPickup()), () => { return !IsBusy; });
        }


        private async Task ConfirmPickup()
        {
            try
            {
                this.IsBusy = true;
                if(await this.UserDialogs.ConfirmAsync(StringResources.ConfirmPickup,StringResources.Delivery))
                {
                    await _deliveryCore.ConfirmDeliveryPickUp(delivery.DeliveryID);
                    delivery.DeliveryStatus = Portol.Common.Helper.DeliveryStatus.InProgress;
                    GetDeliveryStatus();
                    ProcessDelivery();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "ConfirmPickup");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {  
                delivery = (DeliveryDto)navigationData;
                ProcessDelivery();
                GetDriverInformation(delivery.DeliveryID);
                GetDeliveryStatus();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }

        private  void ProcessDelivery()
        {
            try
            {
                DriverRating = 0;
                IsPickedupButtonVisible = false;
                IsSearchingDriver = false;
                IsRatingVisible = false;
                _stopDeliveryStatusLoop = true;
                switch (delivery.DeliveryStatus)
                {
                    case Portol.Common.Helper.DeliveryStatus.SearchingDriver:
                        IsSearchingDriver = true;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.PickingUp:
                        IsPickedupButtonVisible = true;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.InProgress:
                        this.DeliveryStatusMessage = StringResources.ParcelOnBoard;
                        _stopDeliveryStatusLoop = false;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.Delivered:                       
                        this.DeliveryStatusMessage = StringResources.ParcelDelivered;
                        IsRatingVisible = true;
                        break;
                    default:
                        break;
                }               

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "ProcessDelivery");
            }
        }

        private async Task GetDeliveryStatus()
        {
            try
            {
                if(_stopDeliveryStatusLoop)
                {
                    return;
                }

                DateTime initialTime = DateTime.Now;
                await Task.Delay(5000);
                do
                {

                    delivery.DeliveryStatus = await _deliveryCore.GetDeliveryStatus(delivery.DeliveryID);
                    ProcessDelivery();
                    await Task.Delay(100000);

                } while (!_stopDeliveryStatusLoop);              
            }
            catch (Exception ex)
            {               
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "GetDeliveryStatus");
            }
        }

        private async Task GetDriverInformation(Guid deliveryID)
        {
            try
            {
                DateTime initialTime = DateTime.Now;

                while ((DateTime.Now - initialTime).Minutes < 1 && DriverInfo == null)
                {                   
                    DriverInfo = await _deliveryCore.GetDeliveryDriverInfo(deliveryID);
                }
                this.IsSearchingDriver = false;
            }
            catch (Exception ex)
            {
                this.IsSearchingDriver = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "GetDriverInformation");
            }
        }
    }
}
