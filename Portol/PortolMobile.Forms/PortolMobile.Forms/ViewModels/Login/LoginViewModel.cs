using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.SignUp;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {

      
        private readonly ILoginCore _loginCore;

        public ICommand LoginButtonCommand { get; private set; }
        public ICommand RecoverButtonCommand { get; private set; }
        public ICommand SignupCommand { get; private set; }

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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        ISessionData _sessionData;

        public LoginViewModel(ILoginCore userCore, INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData ) : base(navigationService, userDialogs)
        {
          
            _loginCore = userCore;
            LoginButtonCommand = new Command(LoginUser, () => { return !IsBusy; });
            RecoverButtonCommand = new Command(GoToRecoverPassword, () => { return !IsBusy; });
            SignupCommand = new Command(GoToSignup, () => { return !IsBusy; });
            _sessionData = sessionData;

            this.EmailText = "cristhyan@msn.com";
            this.PasswordText = "asd";

        }


        private async void GoToSignup()
        {
            try
            {
                this.IsBusy = true;
                await NavigationService.NavigateToAsync<SignupStepMobileViewModel>();
               
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToSignup");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void GoToRecoverPassword()
        {
            try
            {
                Portol.Common.Interfaces.IPaymentService paymentService = ViewModelLocator.Resolve<Portol.Common.Interfaces.IPaymentService>();
                paymentService.GetCustomers();
                //this.IsBusy = true;
                //await NavigationService.NavigateToAsync<RecoverPasswordViewModel>();
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToRecoverPassword");
            }
            finally
            {
                IsBusy = false;
            }

        }

        private async void LoginUser()
        {
            try
            {

                this.IsBusy = true;
                if (this.PasswordText?.Length > 0 && this.EmailText?.Length > 0)
                {
                    await _sessionData.LoginUser(_loginCore, this.EmailText, this.PasswordText);                   
                    await NavigationService.NavigateToAsync<DropViewModel>();
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
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "LoginUser");
            }
            finally
            {
                this.IsBusy = false;
            }
           
        }
    }
}
