using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.SignUp
{
    public class SignupStepMobileViewModel : BaseViewModel
    {
        public ICommand GotoCodePageCommand { get; private set; }    
        private readonly ILoginService _loginService;
        public ICommand SelectCountryCommand { get; private set; }

        List<CountryDto> _countryItems;
        public List<CountryDto> CountryItems
        {
            get
            {
                return _countryItems;
            }
            set
            {
                _countryItems = value;
                OnPropertyChanged();
            }
        }

        CountryDto _countrySelected;
        public CountryDto CountrySelected
        {
            get
            {
                return _countrySelected;
            }
            set
            {
                _countrySelected = value;
                OnPropertyChanged();
            }
        }

        bool _isMobileValidationVisible;
        public bool IsMobileValidationVisible
        {
            get
            {
                return _isMobileValidationVisible;
            }
            set
            {
                _isMobileValidationVisible = value;
                OnPropertyChanged();
            }
        }


        string _validationMessage;
        public string ValidationMessage
        {
            get
            {
                return _validationMessage;
            }
            set
            {
                _validationMessage = value;
                OnPropertyChanged();
            }
        }


        private string _mobileNumber;
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                long num = 0;
                if (long.TryParse(value, out num))
                {
                    _mobileNumber = value;
                }
                else
                {
                    _mobileNumber = "";
                }

                OnPropertyChanged();
            }
        }

        public SignupStepMobileViewModel(ILoginService loginService)
        {
           
            _loginService = loginService;
            CountryItems = new List<CountryDto>(Constants.CountryList);
            CountrySelected = CountryItems.Where(x => x.Country == EnumCountries.Australia).FirstOrDefault();
            GotoCodePageCommand = new Command(GotoCodePage);
            SelectCountryCommand = new Command(OpenCountryList);
        }

        private void OpenCountryList()
        {
            try
            {
                var cfg = new ActionSheetConfig()
                   .SetTitle(StringResources.Countries);
                foreach (var item in CountryItems)
                {
                    cfg.Add(
                       item.CountryName,
                        () =>
                        {
                            this.CountrySelected = item;
                        });
                }

                cfg.SetCancel(null);
                var disp = this.UserDialogs.ActionSheet(cfg);
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "SaveNewPassword");
            }
        }

        private async void GotoCodePage()
        {
            try
            {
                IsMobileValidationVisible = false;
                if (string.IsNullOrEmpty(MobileNumber))
                {
                    IsMobileValidationVisible = true;
                    ValidationMessage = StringResources.MobileNumberRequiered;
                    return;
                }

                this.IsBusy = true;
                CustomerDto user = new CustomerDto();
                user.PhoneCountryCode = int.Parse(this.CountrySelected.CountryCode);
                user.PhoneNumber = long.Parse(MobileNumber);

                var isUniqui = await _loginService.VerifyMobileUniqueness(user.PhoneNumber, user.PhoneCountryCode);

                if (!isUniqui)
                {
                    IsMobileValidationVisible = true;
                    ValidationMessage = StringResources.MobileInUse;
                    return;
                }

                //var result = await _loginService.SendVerificationCode(user.PhoneNumber, user.PhoneCountryCode);
                //if (result)
                //{
                    await NavigationService.NavigateToAsync<SignupStepCodeViewModel>(user);
                //}
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoCodePage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
