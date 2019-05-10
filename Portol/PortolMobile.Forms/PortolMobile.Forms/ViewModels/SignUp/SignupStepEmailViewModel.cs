using Portol.Common;
using Portol.Common.Interfaces.PortolMobile;
using Portol.Common.DTO;
using PortolMobile.Forms.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using PortolMobile.Forms.Services.Navigation;
using Acr.UserDialogs;

namespace PortolMobile.Forms.ViewModels.SignUp
{
    public class SignupStepEmailViewModel : BaseViewModel
    {
        public ICommand GotoAddressPageCommand { get; private set; }     
        private readonly ILoginService _loginService;
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

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _confirmEmail;
        public string ConfirmEmail
        {
            get
            {
                return _confirmEmail;
            }
            set
            {
                _confirmEmail = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }


        public SignupStepEmailViewModel(ILoginService loginService, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {           
            _loginService = loginService;
            GotoAddressPageCommand = new Command(GotoAddressPage, () => { return !IsBusy; });
        }

        private async void GotoAddressPage()
        {
            try
            {
                this.IsBusy = true;
                IsValidationVisible = false;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.ConfirmEmail) ||
                    string.IsNullOrWhiteSpace(this.Password) || string.IsNullOrWhiteSpace(this.ConfirmPassword))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.AllFieldsRequired;
                    return;
                }
                this.Email = this.Email.Trim().ToLowerInvariant();
                this.ConfirmEmail = this.ConfirmEmail.Trim().ToLowerInvariant();

                if (!this.Email.Contains("@"))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.IncorrectEmailformat;
                    return;
                }

                if (!this.Email.Equals(this.ConfirmEmail))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.NotMatchEmails;
                    return;
                }

                if (!this.Password.Equals(this.ConfirmPassword))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.NotMatchPasswords;
                    return;
                }

                var result = await _loginService.VerifyEmailUniqueness(this.Email);
                if (!result)
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.EmailInUse;
                    return;
                }

                _userDto.Password = this.Password;
                _userDto.Email = this.Email;
                await NavigationService.NavigateToAsync<SignupStepAddressViewModel>(_userDto);

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoAddressPage");
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
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoAddressPage");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
