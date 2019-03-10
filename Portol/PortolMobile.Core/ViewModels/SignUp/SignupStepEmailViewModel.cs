using MvvmCross.Commands;
using MvvmCross.Navigation;
using Portol.Common;
using Portol.Common.Interfaces.PortolMobile;
using Portol.DTO;
using PortolMobile.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.ViewModels.SignUp
{
   public class SignupStepEmailViewModel : BaseViewModel<UserDto>
    {
        public IMvxCommand GotoAddressPageCommand { get; private set; }
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;
        UserDto _userDto;

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
                RaisePropertyChanged(() => IsValidationVisible);
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
                RaisePropertyChanged(() => ErrorMessage);
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
                RaisePropertyChanged(() => Email);
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
                RaisePropertyChanged(() => ConfirmEmail);
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
                RaisePropertyChanged(() => Password);
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
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }


        public SignupStepEmailViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            GotoAddressPageCommand = new MvxAsyncCommand(GotoAddressPage);
        }

        private async Task GotoAddressPage()
        {
            try
            {
                this.IsBusy = true;
                IsValidationVisible = false;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(this.Email ) || string.IsNullOrWhiteSpace(this.ConfirmEmail ) || 
                    string.IsNullOrWhiteSpace(this.Password ) || string.IsNullOrWhiteSpace(this.ConfirmPassword  ))
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

                if (!this.Email.Equals(this.ConfirmEmail ))
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
                if(!result)
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.EmailInUse;
                    return;
                }

                _userDto.Password = this.Password;
                _userDto.Email = this.Email;
                 await _navigationService.Navigate<SignupStepAddressViewModel, UserDto>(_userDto);
               
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

        public override void Prepare(UserDto parameter)
        {
            _userDto = parameter;
        }
    }
}
