using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Login
{
  public  class RecoverPasswordViewModel : BaseViewModel
    {
        public ICommand VerifyCodeButtonCommand { get; private set; }
        public ICommand ReSendCodeButtonCommand { get; private set; }
        public ICommand SendCodeButtonCommand { get; private set; }
        public ICommand SaveNewPasswordCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand SelectCountryCommand { get; private set; }
        
       
        private readonly ILoginService _loginService;

        private string _newPassword;
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _confirmNewPassword;
        public string ConfirmNewPassword
        {
            get
            {
                return _confirmNewPassword;
            }
            set
            {
                _confirmNewPassword = value;
                OnPropertyChanged();
            }
        }

        private string _firstNumber;
        public string FirstNumber
        {
            get
            {
                return _firstNumber;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _firstNumber = value;
                    }
                }
                else
                {
                    _firstNumber = value;
                }

                OnPropertyChanged();

            }
        }

        private string _secondNumber;
        public string SecondNumber
        {
            get
            {
                return _secondNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _secondNumber = value;
                    }
                }
                else
                {
                    _secondNumber = value;
                }

                OnPropertyChanged();
            }
        }

        private string _thirdNumber;
        public string ThirdNumber
        {
            get
            {
                return _thirdNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _thirdNumber = value;
                    }
                }
                else
                {
                    _thirdNumber = value;
                }

                OnPropertyChanged();
            }
        }

        private string _fourNumber;
        public string FourNumber
        {
            get
            {
                return _fourNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _fourNumber = value;
                    }
                }
                else
                {
                    _fourNumber = value;
                }

                OnPropertyChanged();
            }
        }

        private bool _isMobileSectionVisible = true;
        public bool IsMobileSectionVisible
        {
            get
            {
                return _isMobileSectionVisible;
            }
            set
            {
                if (value)
                {
                    IsCodeSectionVisible = false;
                    IsPasswordSectionVisible = false;

                }
                _isMobileSectionVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isCodeSectionVisible = false;
        public bool IsCodeSectionVisible
        {
            get
            {
                return _isCodeSectionVisible;
            }
            set
            {
                if (value)
                {
                    IsMobileSectionVisible = false;
                    IsPasswordSectionVisible = false;

                }
                _isCodeSectionVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isPasswordSectionVisible = false;
        public bool IsPasswordSectionVisible
        {
            get
            {
                return _isPasswordSectionVisible;
            }
            set
            {
                if (value)
                {
                    IsCodeSectionVisible = false;
                    IsMobileSectionVisible = false;

                }
                _isPasswordSectionVisible = value;
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

        public RecoverPasswordViewModel(ILoginService loginService, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
           
            _loginService = loginService;
            CountryItems = new List<CountryDto>(Constants.CountryList);
            CountrySelected = CountryItems.Where(x => x.Country == EnumCountries.Australia).FirstOrDefault();
            SendCodeButtonCommand = new Command(SendCodeVerification, () => { return !IsBusy; });
            ReSendCodeButtonCommand = new Command(ResendCode, () => { return !IsBusy; });
            VerifyCodeButtonCommand = new Command(VerifyCodeVerification, () => { return !IsBusy; });
            SaveNewPasswordCommand = new Command(SaveNewPassword, () => { return !IsBusy; });
            SelectCountryCommand = new Command(OpenCountryList, () => { return !IsBusy; });
            LoginCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LoginViewModel>();
               
            }, () => { return !IsBusy; });

            this.IsMobileSectionVisible = true;
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
                        () => {
                            this.CountrySelected = item;
                        },
                       item.CountryFlagFile
                        );
                }

                cfg.SetCancel(null);
                var disp = this.UserDialogs.ActionSheet(cfg);
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "SaveNewPassword");
            }
        }

        private async void SaveNewPassword()
        {
            try
            {
                if (string.IsNullOrEmpty(this.NewPassword) || string.IsNullOrEmpty(this.ConfirmNewPassword))
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.NewPasswordsRequired,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }

                if (!this.NewPassword.Trim().Equals(this.ConfirmNewPassword.Trim()))
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.PasswordsNotEquals,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }

                this.IsBusy = true;

                await _loginService.ResetNewPassword(Int32.Parse(this.MobileNumber), this.NewPassword);

                UserDialogs.Alert(new AlertConfig
                {
                    Message = StringResources.PasswordChanged,
                    Title = StringResources.RecoveringPassword,
                    OkText = StringResources.Ok
                });

                await NavigationService.NavigateToAsync<LoginViewModel>();
              
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "SaveNewPassword");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void VerifyCodeVerification()
        {
            try
            {
                this.IsBusy = true;
                if (this.FirstNumber == null || this.SecondNumber == null || this.ThirdNumber == null || this.FourNumber == null)
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.CodeNumberRequired,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }
                var code = Int16.Parse(this.FirstNumber + this.SecondNumber + this.ThirdNumber + this.FourNumber);

             
                //var resutl = await _loginService.VerifyCode(Int32.Parse(this.MobileNumber), (int)CountrySelected.Country, code);
                //if (resutl)
                //{
                    this.IsPasswordSectionVisible = true;
                //}
            }

            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "VerifyCodeVerification");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void ResendCode()
        {
            this.IsMobileSectionVisible = true;
        }

        private async void SendCodeVerification()
        {
            try
            {
                this.IsBusy = true;
                if ((string.IsNullOrEmpty(this.MobileNumber) || decimal.Parse(this.MobileNumber) == 0))
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.MobileNumberRequiered,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }

                var isUniqui = await _loginService.VerifyMobileUniqueness(Int32.Parse(this.MobileNumber), (int)CountrySelected.Country);

                if (isUniqui)
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.PhoneNotExist,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });
                    return;
                }

                //var resutl = await _loginService.SendVerificationCode(Int32.Parse(this.MobileNumber), (int)CountrySelected.Country);
                //if (resutl)
                //{
                    this.IsCodeSectionVisible = true;
                //}
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "SendCodeVerification");
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
