using MvvmCross.Commands;
using MvvmCross.Navigation;
using Portol.Common;
using Portol.DTO;
using PortolMobile.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.ViewModels.SignUp
{
    public class SignupStepDetailsViewModel : BaseViewModel<UserDto>
    {
        public IMvxCommand GotoEmailPageCommand { get; private set; }
        private readonly IMvxNavigationService _navigationService;
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

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

       
        private string _dateOBirthStr = DateTime.Now.AddYears(-18).ToShortDateString();
        public string DateOBirthStr
        {
            get
            {
                return _dateOBirthStr;
            }
            set
            {               
                _dateOBirthStr = value;
                RaisePropertyChanged(() => DateOBirthStr);
            }
        }

        public SignupStepDetailsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            GotoEmailPageCommand = new MvxAsyncCommand(GotoEmailPage);
        }

        private async Task GotoEmailPage()
        {
            try
            {
                this.IsBusy = true;
                IsValidationVisible = false;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(this.FirstName ) || string.IsNullOrWhiteSpace(this.LastName ) || string.IsNullOrWhiteSpace(this.DateOBirthStr ))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.AllFieldsRequired;
                    return;
                }

                DateTime dateSelected;
                if (!DateTime.TryParse(this.DateOBirthStr, out dateSelected))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.IncorrectdateFormat;
                    return;
                }

                _userDto.FirstName = this.FirstName;
                _userDto.LastName = this.LastName;
                _userDto.DOB = dateSelected;
                await _navigationService.Navigate<SignupStepEmailViewModel, UserDto>(_userDto);
                await _navigationService.Close(this);
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoEmailPage");
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
