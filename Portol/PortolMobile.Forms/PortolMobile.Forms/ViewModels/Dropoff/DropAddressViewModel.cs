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

        DropoffDto DropoffDetails { get; set; }

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

        INavigationService navigationService;
        IUserDialogs userDialogs;
        public ICommand AddressEntryCommand { get; private set; }



        public DropAddressViewModel(INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
            navigationService = _navigationService;
            userDialogs = _userDialogs;
            AddressEntryCommand = new Command<string>(((x) => GotoAddressPage(x)));
            GotoPicturesCommand = new Command((() => GotoPicturesPage()));
        }

        public async Task GotoPicturesPage()
        {
            try
            {
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


                DropoffDetails.PickupAddress = this.PickUpAddress;
                DropoffDetails.DropoffAddress = this.DropoffAddress;
                await this.NavigationService.NavigateToAsync<DropPicturesViewModel>(DropoffDetails);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "GotoPicturesPage");
            }
        }

        public async Task GotoAddressPage(string typeofaddress)
        {
            try
            {
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
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                DropoffDetails = (DropoffDto)navigationData;
                if (DropoffDetails.Receiver!=null && DropoffDetails.Receiver.CustomerAddress != null)
                {
                    PickUpAddress = DropoffDetails.Receiver.CustomerAddress;
                }
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
