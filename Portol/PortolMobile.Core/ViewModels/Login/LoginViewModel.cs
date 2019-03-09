using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Portol.Common.Interfaces.PortolMobile;
using Portol.Common.Helper;
using Portol.Common;
using PortolMobile.Core.ViewModels.SignUp;
using PortolMobile.Core.Helper;

namespace PortolMobile.Core.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        public IMvxCommand LoginButtonCommand { get; private set; }
        public IMvxCommand RecoverButtonCommand { get; private set; }
        public IMvxCommand SignupCommand { get; private set; }
        
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
            SignupCommand = new MvxAsyncCommand(GoToSignup);
        }


        private async Task GoToSignup()
        {
            try
            {
                await _navigationService.Navigate<SignupStepMobileViewModel>();
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToSignup");
            }
        }

        private async Task GoToRecoverPassword()
        {
            try
            {
                await _navigationService.Navigate<RecoverPasswordViewModel>();
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToRecoverPassword");
            }
               
        }

        private async Task LoginUser()
        {
            try
            {
                
                this.IsBusy = true;
                if(this.PasswordText?.Length>0 && this.EmailText?.Length>0)
                {
                    var result = await _loginService.Authenticate(this.EmailText, this.PasswordText);
                    await _navigationService.Navigate<FirstPageViewModel>();
                }
                else
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.PasswordEmailRequired,
                        Title = StringResources.Login,
                        OkText = StringResources.Ok
                    });

                    return;
                }

               
            }           
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login , "LoginUser");              
            }
            finally
            {
                this.IsBusy = false;
            }
         //  
        }
    }
}
