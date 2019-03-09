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

namespace PortolMobile.Core.ViewModels.Login
{
    public class RecoverPasswordViewModel: BaseViewModel
    {
        public IMvxCommand VerifyCodeButtonCommand { get; private set; }
        public IMvxCommand ReSendCodeButtonCommand { get; private set; }
        public IMvxCommand SendCodeButtonCommand { get; private set; }
        public IMvxCommand SaveNewPasswordCommand { get; private set; }
        public IMvxCommand LoginCommand { get; private set; }

        
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

        private string _codeNumber;
        public string CodeNumber
        {
            get
            {
                return _codeNumber;
            }
            set
            {
                int num = 0;
                if (int.TryParse(value, out num))
                {
                    _codeNumber = value;
                }
                else
                {
                    _codeNumber = "";
                }

                RaisePropertyChanged(() => MobileNumber);
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

        public RecoverPasswordViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            SendCodeButtonCommand = new MvxAsyncCommand(SendCodeVerification);
            ReSendCodeButtonCommand = new MvxCommand(ResendCode);
            VerifyCodeButtonCommand = new MvxAsyncCommand(VerifyCodeVerification);
            SaveNewPasswordCommand = new MvxAsyncCommand(SaveNewPassword);
            LoginCommand = new MvxAsyncCommand(async () =>
            {
              await  _navigationService.Navigate<LoginViewModel>();
            });
            this.IsMobileSectionVisible = true;
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

                 _navigationService.Navigate<LoginViewModel>();
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
                if (string.IsNullOrEmpty(this.CodeNumber) || int.Parse(this.CodeNumber) == 0)
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.CodeNumberRequired,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }
                var resutl = await _loginService.VerifyCode(decimal.Parse(this.MobileNumber), int.Parse(this.CodeNumber) );
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
                if ((string.IsNullOrEmpty(this.MobileNumber) || decimal.Parse(this.MobileNumber)==0) && string.IsNullOrEmpty(this.CodeNumber))
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.MobileNumberRequiered,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }
                var resutl = await _loginService.SendVerificationCode(decimal.Parse(this.MobileNumber), int.Parse(this.CodeNumber));
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
