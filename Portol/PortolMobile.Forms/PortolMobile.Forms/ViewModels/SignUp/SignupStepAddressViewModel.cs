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

        //private string _city;
        //public string City
        //{
        //    get
        //    {
        //        return _city;
        //    }
        //    set
        //    {
        //        _city = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        IAddressService _addressService;

        public SignupStepAddressViewModel(IUserCore userCore, IAddressService addressService, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            _userCore = userCore;           
            SaveAccountCommand = new Command(GetPosibleAddresses, () => { return !IsBusy; });
            _addressService = addressService;
            this.Country = "AU";
        }

        private async void GetPosibleAddresses()
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

                if (string.IsNullOrWhiteSpace(this.PostCode))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.PostCodeRequired;
                    return;
                }

                //if (string.IsNullOrWhiteSpace(this.City))
                //{
                //    IsValidationVisible = true;
                //    ErrorMessage = StringResources.CityRequired;
                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(this.State))
                //{
                //    IsValidationVisible = true;
                //    ErrorMessage = StringResources.StateRequired;
                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(this.Country))
                //{
                //    IsValidationVisible = true;
                //    ErrorMessage = StringResources.CountryRequired;
                //    return;
                //}

                _userDto.CustomerAddress = new AddressDto();            
                _userDto.CustomerAddress.Country = this.Country;
                _userDto.CustomerAddress.FlatNumber = this.UnitNumber;
                _userDto.CustomerAddress.State = this.State;
                _userDto.CustomerAddress.StreetName = this.Street;
                _userDto.CustomerAddress.Suburb = this.Suburb;
                _userDto.CustomerAddress.PostCode = this.PostCode;

                var result = await _addressService.GetPosibleAddresses(_userDto.CustomerAddress.FullAddress );
                if (result == null || result.completions == null || result.completions.Count == 0)
                {
                   // _userDto.CustomerAddress.City = "";
                    _userDto.CustomerAddress.Country = "";
                    _userDto.CustomerAddress.FlatNumber = this.UnitNumber;
                    _userDto.CustomerAddress.State = "";
                    _userDto.CustomerAddress.StreetName = this.Street;
                    _userDto.CustomerAddress.Suburb = "";
                    _userDto.CustomerAddress.PostCode = this.PostCode;
                    result = await _addressService.GetPosibleAddresses(_userDto.CustomerAddress.FullAddress );
                }

                if (result != null && result.completions?.Count > 0)
                {
                    var cfg = new ActionSheetConfig()
                    .SetTitle(StringResources.SelectAddress);

                    var noAddress = new AddressFinderResultDto();
                    noAddress.full_address = StringResources.UserSuggestedAddress;
                    noAddress.id = "";
                    result.completions.Insert(0, noAddress);

                    foreach (var item in result.completions)
                    {
                        cfg.Add(
                           item.full_address,
                            () =>
                            {
                                GetAddressMetadata(item.id);
                            });
                    }
                                      

                    cfg.SetCancel(null);
                    var disp = this.UserDialogs.ActionSheet(cfg);

                }
                else
                {
                    _userDto.CustomerAddress.Country = this.Country;
                    _userDto.CustomerAddress.FlatNumber = this.UnitNumber;
                    _userDto.CustomerAddress.State = this.State;
                    _userDto.CustomerAddress.StreetName = this.Street;
                    _userDto.CustomerAddress.Suburb = this.Suburb;
                    _userDto.CustomerAddress.PostCode = this.PostCode;

                }

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GetPosibleAddresses");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


        private async Task GetAddressMetadata(string addressId)
        {
            try
            {
                this.IsBusy = true;
                if(!string.IsNullOrEmpty(addressId))
                {
                    var result = await _addressService.GetAddressMetadata(addressId);
                    if (result != null)
                    {
                        _userDto.CustomerAddress = result.GetAddressDto();
                    }
                }
                else
                {
                    _userDto.CustomerAddress.Country = this.Country;
                    _userDto.CustomerAddress.FlatNumber = this.UnitNumber;
                    _userDto.CustomerAddress.State = this.State;
                    _userDto.CustomerAddress.StreetName = this.Street;
                    _userDto.CustomerAddress.Suburb = this.Suburb;
                    _userDto.CustomerAddress.PostCode = this.PostCode;
                }

                await SaveNewAccount();
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GetAddressMetadata");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task SaveNewAccount()
        {
            try
            {
                this.IsBusy = true;                 

                if (await _userCore.CreateNewCustomer(_userDto))
                {
                    await UserDialogs.ConfirmAsync(StringResources.AccountCreated, StringResources.NewUser);
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
                _userDto = (CustomerDto)navigationData;
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
