using Portol.Common;
using Portol.DTO;
using PortolMobile.Forms.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.SignUp
{
    public class SignupStepDetailsViewModel : BaseViewModel
    {
        public ICommand GotoEmailPageCommand { get; private set; }
     
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }


        private DateTime _dateOBirth;
        public DateTime DateOBirth
        {
            get
            {
                return _dateOBirth;
            }
            set
            {
                _dateOBirth = value;
                OnPropertyChanged();
            }
        }

        public SignupStepDetailsViewModel()
        {           
            GotoEmailPageCommand = new Command(GotoEmailPage);
            this.DateOBirth = DateTime.Now.AddYears(-18);
        }

        private async void GotoEmailPage()
        {
            try
            {
                this.IsBusy = true;
                IsValidationVisible = false;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(this.FirstName) || string.IsNullOrWhiteSpace(this.LastName))
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.AllFieldsRequired;
                    return;
                }

                //DateTime dateSelected;
                //if (!DateTime.TryParse(this.DateOBirth, out dateSelected))
                //{
                //    IsValidationVisible = true;
                //    ErrorMessage = StringResources.IncorrectdateFormat;
                //    return;
                //}

                _userDto.FirstName = this.FirstName;
                _userDto.LastName = this.LastName;
                _userDto.DOB = DateOBirth;
                await NavigationService.NavigateToAsync<SignupStepEmailViewModel>(_userDto);

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
             
        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                _userDto = (UserDto)navigationData; 
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoEmailPage");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
