using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
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

        private CustomerDto _receiver;
        public CustomerDto Receiver
        {
            get
            {
                return _receiver;
            }
            set
            {
                _receiver = value;
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
                    if(_pickUpAddress==null)
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
            AddressEntryCommand = new Command<string>(GotoAddressPage);
        }

        private async void GotoAddressPage(string typeofaddress)
        {
            try
            {
                AddressDto address;
                if (typeofaddress.Equals("pickup"))
                {
                    address = this.PickUpAddress;
                }
                else
                {
                    address = this.DropoffAddress;
                }

                await this.NavigationService.NavigateToAsync<AddressPickerViewModel>(address);
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
                Receiver = (CustomerDto)navigationData;
                if(Receiver.CustomerAddress !=null)
                {                   
                    PickUpAddress = Receiver.CustomerAddress;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "InitializeAsync");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
