using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.Interfaces.PortolMobile;
using Portol.DTO;
using PortolMobile.Forms.Helper;
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
      
        private readonly IUserMobileService _userMobileService;


        UserDto _userDto;

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

        private string _unitNumber;
        public string UnitNumber
        {
            get
            {
                return _unitNumber;
            }
            set
            {
                _unitNumber = value;
                OnPropertyChanged();
            }
        }

        private string _street;
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }

        private string _suburb;
        public string Suburb
        {
            get
            {
                return _suburb;
            }
            set
            {
                _suburb = value;
                OnPropertyChanged();
            }
        }

        private string _postCode;
        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                _postCode = value;
                OnPropertyChanged();
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        public SignupStepAddressViewModel( IUserMobileService userMobileService)
        {
            _userMobileService = userMobileService;           
            SaveAccountCommand = new Command(SaveNewAccount);
        }

        private async void SaveNewAccount()
        {
            try
            {
                this.IsBusy = true;
                IsValidationVisible = false;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(this.Street))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.StreetRequired;
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Suburb))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.SuburbRequired;
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.City))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.CityRequired;
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.State))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.StateRequired;
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Country))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.CountryRequired;
                    return;
                }

                _userDto.UserAddress = new AddressDto();
                _userDto.UserAddress.City = this.City;
                _userDto.UserAddress.Country = this.Country;
                _userDto.UserAddress.FlatNumber = this.UnitNumber;
                _userDto.UserAddress.State = this.State;
                _userDto.UserAddress.StreetName = this.Street;
                _userDto.UserAddress.Suburb = this.Suburb;

                if (await _userMobileService.CreateNewuser(_userDto))
                {
                    await UserDialogs.ConfirmAsync(StringResources.AccountCreated, StringResources.NewUser);
                    await NavigationService.GoToMainPage();

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
                _userDto = (UserDto)navigationData;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "SaveNewAccount");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }      
    }
}
