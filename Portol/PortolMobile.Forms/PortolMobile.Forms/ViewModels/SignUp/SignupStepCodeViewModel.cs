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
    public class SignupStepCodeViewModel : BaseViewModel
    {
        public ICommand GotoNamesPageCommand { get; private set; }     
        private readonly ILoginService _loginService;

        private string _mobileNumber;
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }

        private string _firstNumber;
        public string FirstNumber
        {
            get
            {
                return _firstNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _firstNumber = value;
                    }
                }
                else
                {
                    _firstNumber = value;
                }

                OnPropertyChanged();

            }
        }

        private string _secondNumber;
        public string SecondNumber
        {
            get
            {
                return _secondNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _secondNumber = value;
                    }
                }
                else
                {
                    _secondNumber = value;
                }

                OnPropertyChanged();
            }
        }

        private string _thirdNumber;
        public string ThirdNumber
        {
            get
            {
                return _thirdNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _thirdNumber = value;
                    }
                }
                else
                {
                    _thirdNumber = value;
                }

                OnPropertyChanged();
            }
        }

        private string _fourNumber;
        public string FourNumber
        {
            get
            {
                return _fourNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Int16 intVal = Int16.Parse(value);
                    if (intVal < 10 && intVal >= 0)
                    {
                        _fourNumber = value;
                    }
                }
                else
                {
                    _fourNumber = value;
                }

                OnPropertyChanged();
            }
        }

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

        CustomerDto _userDto;

        public SignupStepCodeViewModel(  ILoginService loginService, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {          
            _loginService = loginService;
            GotoNamesPageCommand = new Command(GotoNamesPage, () => { return !IsBusy; });
        }

        private async void GotoNamesPage()
        {
            try
            {
                IsValidationVisible = false;
                ErrorMessage = "";

                if (this.FirstNumber == null || this.SecondNumber == null || this.ThirdNumber == null || this.FourNumber == null)
                {
                    IsValidationVisible = true;
                    ErrorMessage = StringResources.WrongCode;
                    return;
                }

                this.IsBusy = true;

                var code = Int16.Parse(this.FirstNumber + this.SecondNumber + this.ThirdNumber + this.FourNumber);

               // await _loginService.VerifyCode(_userDto.PhoneNumber, _userDto.PhoneCountryCode, code);

                await NavigationService.NavigateToAsync<SignupStepDetailsViewModel>(_userDto);
             
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoNamesPage");
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
                _userDto =(CustomerDto)navigationData;
                if (_userDto != null)
                {
                    MobileNumber = "+" + _userDto.PhoneCountryCode.ToString() + _userDto.PhoneNumber.ToString();
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoNamesPage");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
