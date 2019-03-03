using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using MvvmCross.Logging;
using System.Threading.Tasks;
using Portol.Common;
using MvvmCross.UI;

namespace PortolMobile.Core.ViewModels.Login
{
    public class RecoverPasswordViewModel: BaseViewModel
    {
        public IMvxCommand VerifyCodeButtonCommand { get; private set; }
        public IMvxCommand ReSendCodeButtonCommand { get; private set; }
        public IMvxCommand SendCodeButtonCommand { get; private set; }


        

        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

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
                _isCodeSectionVisible = value;
                RaisePropertyChanged(() => IsCodeSectionVisible);
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
                int num = 0;
                if (int.TryParse(value, out num))
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
            this.IsMobileSectionVisible = true;
            this.IsCodeSectionVisible = false ;
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
                var resutl = await _loginService.VerifyCode(int.Parse(this.MobileNumber), int.Parse(this.CodeNumber) );
                if (resutl)
                {
                   
                }
            }
            catch (AppException ex)
            {
                UserDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = StringResources.RecoveringPassword,
                    OkText = StringResources.Ok
                });
            }
            catch (System.Exception ex)
            {
                Logs.Instance.ErrorException("SendCodeVerification", ex);
                UserDialogs.Alert(new AlertConfig
                {
                    Message = StringResources.GeneralError,
                    Title = StringResources.Error,
                    OkText = StringResources.Ok
                });
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void ResendCode()
        {
            this.IsMobileSectionVisible = true;
            this.IsCodeSectionVisible = false ;
        }

        private async Task SendCodeVerification()
        {
            try
            {
                this.IsBusy = true;
                if (string.IsNullOrEmpty(this.MobileNumber) || int.Parse(this.MobileNumber)==0)
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.MobileNumberRequiered,
                        Title = StringResources.RecoveringPassword,
                        OkText = StringResources.Ok
                    });

                    return;
                }
                var resutl = await _loginService.SendVerificationCode(int.Parse(this.MobileNumber));
                if(resutl )
                {
                    this.IsMobileSectionVisible = false;
                    this.IsCodeSectionVisible = true ;
                }
            }
            catch (AppException ex)
            {
                UserDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = StringResources.RecoveringPassword,
                    OkText = StringResources.Ok
                });
            }
            catch (System.Exception ex)
            {
                Logs.Instance.ErrorException("SendCodeVerification", ex);
                UserDialogs.Alert(new AlertConfig
                {
                    Message = StringResources.GeneralError,
                    Title = StringResources.Error,
                    OkText = StringResources.Ok
                });
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
