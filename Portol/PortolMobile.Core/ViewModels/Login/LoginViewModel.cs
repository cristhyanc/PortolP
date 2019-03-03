using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Portol.Common.Interfaces.PortolMobile;
using Portol.Common.Helper;
using Portol.Common;

namespace PortolMobile.Core.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        public IMvxCommand LoginButtonCommand { get; private set; }
        public IMvxCommand RecoverButtonCommand { get; private set; }

        private string _emailText;
        public string EmailText
        {
            get
            {
                return _emailText;
            }
            set
            {
                _emailText = value;
                RaisePropertyChanged(() => EmailText);
            }
        }

        private string _passwordText;
        public string PasswordText
        {
            get
            {
                return _passwordText;
            }
            set
            {
                _passwordText = value;
                RaisePropertyChanged(() => PasswordText);
            }
        }

      
        public LoginViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;           
            _loginService = loginService;
            LoginButtonCommand = new MvxAsyncCommand(LoginUser);
            RecoverButtonCommand = new MvxAsyncCommand(GoToRecoverPassword);
            
        }
               
        private async Task GoToRecoverPassword()
        {
            try
            {
                await _navigationService.Navigate<RecoverPasswordViewModel>();
            }
            catch (System.Exception ex)
            {
                Logs.Instance.ErrorException("GoToRecoverPassword", ex);
                UserDialogs.Alert(new AlertConfig
                {
                    Message = StringResources.GeneralError,
                    Title = StringResources.Error,
                    OkText = StringResources.Ok
                });
            }
        }

        private async Task LoginUser()
        {
            try
            {
                
                this.IsBusy = true;
                if(this.PasswordText?.Length==0 || this.EmailText?.Length==0)
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.PasswordEmailRequired,
                        Title = StringResources.Login,
                        OkText = StringResources.Ok
                    });

                    return;
                }

                var result =await  _loginService.Authenticate(this.EmailText, this.PasswordText);
                await _navigationService.Navigate<FirstPageViewModel>();
            }
            catch (AppException ex)
            {
                UserDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = StringResources.Error,
                    OkText = StringResources.Ok
                });
            }
            catch (System.Exception ex)
            {
                Logs.Instance.ErrorException("LoginUser", ex);
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
         //  
        }
    }
}
