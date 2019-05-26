using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropAddressViewModel : BaseViewModel
    {
        
        public ICommand GotoPicturesCommand { get; private set; }

        DeliveryDto DropoffDetails { get; set; }

        string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _pickupAddressStr;
        public string PickupAddressStr
        {
            get
            {
                return _pickupAddressStr;
            }
            set
            {
                _pickupAddressStr = value;
                OnPropertyChanged();
            }
        }

        private string _dropoffAddressStr;
        public string DropoffAddressStr
        {
            get
            {
                return _dropoffAddressStr;
            }
            set
            {
                _dropoffAddressStr = value;
                OnPropertyChanged();
            }
        }

        private AddressDto _pickUpAddress;
        public AddressDto PickUpAddress
        {
            get
            {
                return _pickUpAddress;
            }
            set
            {
                if (_pickUpAddress != value)
                {
                    _pickUpAddress = value;
                    if (_pickUpAddress == null)
                    {
                        PickupAddressStr = "";
                    }
                    else
                    {
                        PickupAddressStr = _pickUpAddress.FullAddress;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private AddressDto _dropoffAddress;
        public AddressDto DropoffAddress
        {
            get
            {
                return _dropoffAddress;
            }
            set
            {
                if (_dropoffAddress != value)
                {
                    _dropoffAddress = value;
                    if (_dropoffAddress == null)
                    {
                        DropoffAddressStr = "";
                    }
                    else
                    {
                        DropoffAddressStr = _dropoffAddress.FullAddress;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string SenderName
        {
            get { return string.Format(StringResources.HelloMessage, _sessionData.User.FirstName); }

        }

        INavigationService _navigationService;
        IUserDialogs _userDialogs;
        ISessionData _sessionData;
        public ICommand AddressEntryCommand { get; private set; }



        public DropAddressViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData) : base(navigationService, userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _sessionData = sessionData;
            AddressEntryCommand = new Command<string>(((x) => GotoAddressPage(x)), (x) => { return !IsBusy; });
            GotoPicturesCommand = new Command((() => GotoPicturesPage()), () => { return !IsBusy; });
            
        }

        public async Task GotoPicturesPage()
        {
            try
            {
                this.IsBusy = true;
                if (PickUpAddress == null || string.IsNullOrEmpty(PickUpAddress.FullAddress))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.PickupAddressRequired);
                    return;
                }

                if (DropoffAddress == null || string.IsNullOrEmpty(DropoffAddress.FullAddress))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.DropoffAddressRequired);
                    return;
                }

                if (string.IsNullOrEmpty(this.Description))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.DescriptionRequired);
                    return;
                }

                DropoffDetails.Description = this.Description;
                DropoffDetails.PickupAddress = this.PickUpAddress;
                DropoffDetails.DropoffAddress = this.DropoffAddress;
                await this.NavigationService.NavigateToAsync<DropPicturesViewModel>(DropoffDetails);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "GotoPicturesPage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public async Task GotoAddressPage(string typeofaddress)
        {
            try
            {
                this.IsBusy = true;
                AddressPickerParameters parameter = new AddressPickerParameters();
                if (typeofaddress.Equals("pickup"))
                {
                    parameter.Address = this.PickUpAddress;
                    parameter.IsPickupAddress = true;
                }
                else
                {
                    parameter.Address = this.DropoffAddress;

                }
                SubscribeMessagingService();
                await this.NavigationService.NavigateToAsync<AddressPickerViewModel>(parameter);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "GotoAddressPage");
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
                DropoffDetails = (DeliveryDto)navigationData;
                if (DropoffDetails.Receiver!=null && DropoffDetails.Receiver.CustomerAddress != null)
                {
                    PickUpAddress = DropoffDetails.Receiver.CustomerAddress;
                }

                //sample
                //_dropoffAddress = new AddressDto();
                //_dropoffAddress.AddressValidated = true;
                //_dropoffAddress.FullAddress = "79 berwick st, fortitude valley 4006";
                //DropoffAddress = _dropoffAddress;
                //Description = "asd";
                //-Sample
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "InitializeAsync");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }

        protected override void PageAppearing()
        {
            UnsubscribeMessagingService();            
        }

        private void UnsubscribeMessagingService()
        {
            try
            {
                MessagingCenter.Unsubscribe<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "UnsubscribeMessagingService");
            }
        }

        private void SubscribeMessagingService()
        {
            try
            {
                MessagingCenter.Subscribe<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage, (sender, arg) =>
                {
                    if (arg != null && arg.Address != null)
                    {
                        if (arg.IsPickupAddress)
                        {
                            this.PickUpAddress = arg.Address;
                        }
                        else
                        {
                            this.DropoffAddress = arg.Address;
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "SubscribeMessagingService");
            }
        }
    }
}
