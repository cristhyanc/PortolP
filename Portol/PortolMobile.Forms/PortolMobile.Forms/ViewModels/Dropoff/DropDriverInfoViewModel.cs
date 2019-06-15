using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
   public class DropDriverInfoViewModel: BaseViewModel
    {
        private List<Pin> _locations;
        public List<Pin> Locations
        {
            get
            {
                return _locations;
            }
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

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

        bool _isCancelOptionVisible;
        public bool IsCancelOptionVisible
        {
            get
            {
                return _isCancelOptionVisible;
            }
            set
            {
                _isCancelOptionVisible = value;
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
                SendRateService();
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

        BottomMenuControlModel _bottomMenuControl;
        public BottomMenuControlModel BottomMenuControl
        {
            get { return _bottomMenuControl; }
            set
            {
                _bottomMenuControl = value;
                OnPropertyChanged();
            }
        }

        
        public ICommand ConfirmPickupCommand { get; private set; }
        public ICommand CancelServiceCommand { get; private set; }

        bool _stopDeliveryStatusLoop;
        DeliveryDto delivery;
        IDeliveryCore _deliveryCore;


        public DropDriverInfoViewModel(INavigationService navigationService, IUserDialogs userDialogs, IDeliveryCore deliveryCore, ISessionData sessionData) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
            ConfirmPickupCommand = new Command((() => ConfirmPickup()), () => { return !IsBusy; });
            BottomMenuControl = new BottomMenuControlModel(navigationService, userDialogs, deliveryCore, sessionData);
            CancelServiceCommand = new Command((() => CancelService()), () => { return !IsBusy; });
        }


        private async void CancelService()
        {
            try
            {
                if (await UserDialogs.ConfirmAsync(StringResources.CancelServiceQuestion))
                {
                    await _deliveryCore.CancelDelivery(delivery.DeliveryID);
                    await this.NavigationService.GoToMainPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "CancelService");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task SendRateService()
        {
            try
            {
                this.IsBusy = true;
                await _deliveryCore.RateDelivery(delivery.DeliveryID, this.DriverRating);
                await this.NavigationService.GoToMainPage();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "SendRateService");
            }
            finally
            {
                this.IsBusy = false;
            }
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
                BottomMenuControl.InitControl();

              
                _locations = new List<Pin>();
                if (delivery != null)
                {

                    double latitude, longitude;
                    Pin pin;
                    if (delivery.PickupAddress != null)
                    {
                        pin = new Pin { Label = StringResources.Pickup };
                        double.TryParse(delivery.PickupAddress.Latitude, out latitude);
                        double.TryParse(delivery.PickupAddress.Longitude, out longitude);
                        Position position = new Position(latitude, longitude);
                        pin.Position = position;
                        pin.Address = delivery.PickupAddress.FullAddress;

                        _locations.Add(pin);
                    }

                    if (delivery.DropoffAddress != null)
                    {
                        pin = new Pin { Label = StringResources.Dropoff };
                        double.TryParse(delivery.DropoffAddress.Latitude, out latitude);
                        double.TryParse(delivery.DropoffAddress.Longitude, out longitude);
                        Position position = new Position(latitude, longitude);
                        pin.Position = position;
                        pin.Address = delivery.DropoffAddress.FullAddress;
                        _locations.Add(pin);

                    }

                    Locations = _locations;
                }



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
              //  DriverRating = 0;
                IsPickedupButtonVisible = false;
                IsSearchingDriver = false;
                IsRatingVisible = false;
                _stopDeliveryStatusLoop = false;
                IsCancelOptionVisible = false;
                switch (delivery.DeliveryStatus)
                {
                    case Portol.Common.Helper.DeliveryStatus.SearchingDriver:
                        IsSearchingDriver = true;
                        IsCancelOptionVisible = true;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.PickingUp:
                        IsPickedupButtonVisible = true;
                        IsCancelOptionVisible = true;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.InProgress:
                        this.DeliveryStatusMessage = StringResources.ParcelOnBoard;
                        //_stopDeliveryStatusLoop = false;
                        break;
                    case Portol.Common.Helper.DeliveryStatus.Delivered:
                        _stopDeliveryStatusLoop = true;
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
                    await Task.Delay(40000);

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
                ProcessDelivery();
                //this.IsSearchingDriver = false;
            }
            catch (Exception ex)
            {
                this.IsSearchingDriver = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "GetDriverInformation");
            }
        }
    }
}
