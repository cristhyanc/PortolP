using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
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

namespace PortolMobile.Forms.ViewModels.SignUp
{
    public class SignupStepAddressViewModel : BaseViewModel
    {

        public ICommand SaveAccountCommand { get; private set; }
      
        private readonly IUserCore _userCore;


        CustomerDto _userDto;

        bool _isValidationVisible;
        public bool IsValidationVisible
        {
            get
            {
                return _isValidationVisible;
            }
            set
            {
                _isValidationVisible = value;
                OnPropertyChanged();
            }
        }

        bool _isAddressVisible;
        public bool IsAddressVisible
        {
            get
            {
                return _isAddressVisible;
            }
            set
            {
                _isAddressVisible = value;
                OnPropertyChanged();
            }
        }

        
        string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        AddressDto _homeAddress;
        public AddressDto HomeAddress
        {
            get
            {
                return _homeAddress;
            }
            set
            {
                if (_homeAddress != value)
                {
                    _homeAddress = value;
                    if (_homeAddress == null)
                    {
                        HomeAddressStr = "";
                    }
                    else
                    {
                        HomeAddressStr = _homeAddress.FullAddress;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private string _homeAddressStr;
        public string HomeAddressStr
        {
            get
            {
                return _homeAddressStr;
            }
            set
            {
                _homeAddressStr = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddressEntryCommand { get; private set; }

        IAddressService _addressService;
        ISessionData _sessionData;
        ILoginCore _loginCore;
        public SignupStepAddressViewModel(IUserCore userCore, IAddressService addressService, INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, ILoginCore loginCore ) : base(navigationService, userDialogs)
        {
            _userCore = userCore;           
            SaveAccountCommand = new Command((() => SaveNewAccount()), () => { return !IsBusy; });
            _addressService = addressService;
            AddressEntryCommand = new Command((() => GotoAddressPage()), () => { return !IsBusy; });
            _sessionData = sessionData;
            _loginCore = loginCore;
           
        }

        protected override void PageAppearing()
        {
            UnsubscribeMessagingService();
        }

        public async Task GotoAddressPage()
        {
            try
            {
                this.IsBusy = true;
                AddressPickerParameters parameter = new AddressPickerParameters();
                parameter.Address = HomeAddress;
                SubscribeMessagingService();
                await this.NavigationService.NavigateToAsync<AddressPickerViewModel>(parameter);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "SignupStepAddressViewModel", "GotoAddressPage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void UnsubscribeMessagingService()
        {
            try
            {
                MessagingCenter.Unsubscribe<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "SignupStepAddressViewModel", "UnsubscribeMessagingService");
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
                        this.HomeAddress = arg.Address;
                     
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "SignupStepAddressViewModel", "SubscribeMessagingService");
            }
        }

      
        private async Task SaveNewAccount()
        {
            try
            {
                this.IsBusy = true;

                if (HomeAddress == null || string.IsNullOrEmpty(HomeAddress.FullAddress))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.AddressRequired);
                    return;
                }
                _userDto.CustomerAddress = this.HomeAddress;
                if (await _userCore.CreateNewCustomer(_userDto))
                {
                   // await UserDialogs.ConfirmAsync(StringResources.AccountCreated, StringResources.NewUser);
                    await _sessionData.LoginUser(_loginCore, _userDto.Email, _userDto.Password);
                    this.IsBusy = false;
                    IsAddressVisible = false;

                    await Task.Delay(2000);
                    await NavigationService.NavigateToAsync<DropViewModel>();

                }
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "SaveNewAccount");
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
                IsAddressVisible = true;
                _userDto = (CustomerDto)navigationData;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "InitializeAsync");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }      
    }
}
