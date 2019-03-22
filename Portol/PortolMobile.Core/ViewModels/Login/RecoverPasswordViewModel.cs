using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using MvvmCross.Logging;
using System.Threading.Tasks;
using Portol.Common;
using MvvmCross.UI;
using PortolMobile.Core.Helper;
using Portol.Common.DTO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PortolMobile.Core.ViewModels.Login
{
    public class RecoverPasswordViewModel: BaseViewModel
    {
        public IMvxCommand VerifyCodeButtonCommand { get; private set; }
        public IMvxCommand ReSendCodeButtonCommand { get; private set; }
        public IMvxCommand SendCodeButtonCommand { get; private set; }
        public IMvxCommand SaveNewPasswordCommand { get; private set; }
        public IMvxCommand LoginCommand { get; private set; }
        public IMvxCommand SelectCountryCommand { get; private set; }
        

        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        private string _newPassword;
        public string NewPassword {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                RaisePropertyChanged(() => NewPassword);
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
                RaisePropertyChanged(() => ConfirmNewPassword);
            }
        }

        private Int16? _firstNumber;
        public Int16? FirstNumber
        {
            get
            {
                return _firstNumber;
            }
            set
            {
                if (value.HasValue && value.Value < 10 && value.Value >= 0)
                {
                    _firstNumber = value;
                }

                RaisePropertyChanged(() => FirstNumber);
            }
        }

        private Int16? _secondNumber;
        public Int16? SecondNumber
        {
            get
            {
                return _secondNumber;
            }
            set
            {
                if (value.HasValue && value.Value < 10 && value.Value >= 0)
                {
                    _secondNumber = value;
                }

                RaisePropertyChanged(() => SecondNumber);
            }
        }

        private Int16? _thirdNumber;
        public Int16? ThirdNumber
        {
            get
            {
                return _thirdNumber;
            }
            set
            {
                if (value.HasValue && value.Value < 10 && value.Value >= 0)
                {
                    _thirdNumber = value;
                }

                RaisePropertyChanged(() => ThirdNumber);
            }
        }

        private Int16? _fourNumber;
        public Int16? FourNumber
        {
            get
            {
                return _fourNumber;
            }
            set
            {
                if (value.HasValue && value.Value < 10 && value.Value >= 0)
                {
                    _fourNumber = value;
                }

                RaisePropertyChanged(() => FourNumber);
            }
        }

        private bool _isMobileSectionVisible=true;
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
                RaisePropertyChanged(() => IsMobileSectionVisible);
            }
        }

        private bool  _isCodeSectionVisible=false;
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
                RaisePropertyChanged(() => IsCodeSectionVisible);
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
                if(value)
                {
                    IsCodeSectionVisible = false;
                    IsMobileSectionVisible = false;

                }
                _isPasswordSectionVisible = value;
                RaisePropertyChanged(() => IsPasswordSectionVisible);
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

                RaisePropertyChanged(() => MobileNumber);
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
                RaisePropertyChanged(() => CountryItems);
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
                RaisePropertyChanged(() => CountrySelected);
            }
        }

        public RecoverPasswordViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            CountryItems = new List<CountryDto>(Constants.CountryList);
            CountrySelected = CountryItems.Where(x => x.Country == EnumCountries.Australia).FirstOrDefault();
            SendCodeButtonCommand = new MvxAsyncCommand(SendCodeVerification);
            ReSendCodeButtonCommand = new MvxCommand(ResendCode);
            VerifyCodeButtonCommand = new MvxAsyncCommand(VerifyCodeVerification);
            SaveNewPasswordCommand = new MvxAsyncCommand(SaveNewPassword);
            SelectCountryCommand = new MvxCommand(OpenCountryList);
            LoginCommand = new MvxAsyncCommand(async () =>
            {
              await  _navigationService.Navigate<LoginViewModel>();
                await _navigationService.Close(this);
            });
           
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

        private async Task SaveNewPassword()
        {
            try
            {
                if(string.IsNullOrEmpty(this.NewPassword) || string.IsNullOrEmpty(this.ConfirmNewPassword) )
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.NewPasswordsRequired,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }

                if(!this.NewPassword.Trim().Equals(this.ConfirmNewPassword.Trim()))
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

                await _loginService.ResetNewPassword(decimal.Parse(this.MobileNumber), this.NewPassword);

                UserDialogs.Alert(new AlertConfig
                {
                    Message = StringResources.PasswordChanged,
                    Title = StringResources.RecoveringPassword,
                    OkText = StringResources.Ok
                });

                await _navigationService.Navigate<LoginViewModel>();
                await _navigationService.Close(this);
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

        private async Task VerifyCodeVerification()
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
                var code = Int16.Parse(this.FirstNumber.Value.ToString() + this.SecondNumber.Value.ToString() + this.ThirdNumber.Value.ToString() + this.FourNumber.Value.ToString());

                var resutl = await _loginService.VerifyCode(decimal.Parse(this.MobileNumber), code);
                if (resutl)
                {
                    this.IsPasswordSectionVisible = true;
                }
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

        private async Task SendCodeVerification()
        {
            try
            {
                this.IsBusy = true;
                if ((string.IsNullOrEmpty(this.MobileNumber) || decimal.Parse(this.MobileNumber)==0) )
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.MobileNumberRequiered,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }
                var resutl = await _loginService.SendVerificationCode(decimal.Parse(this.MobileNumber),(int)CountrySelected.Country);
                if(resutl )
                {                  
                    this.IsCodeSectionVisible = true ;
                }
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
