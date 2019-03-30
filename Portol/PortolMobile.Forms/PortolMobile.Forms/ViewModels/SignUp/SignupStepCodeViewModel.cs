//using MvvmCross.Commands;
//using MvvmCross.Navigation;
//using Portol.Common;
//using Portol.Common.Interfaces.PortolMobile;
//using Portol.DTO;
//using PortolMobile.Core.Helper;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace PortolMobile.Forms.ViewModels.SignUp
//{
//   public class SignupStepCodeViewModel: BaseViewModel<UserDto>
//    {
//        public IMvxCommand GotoNamesPageCommand { get; private set; }
//        private readonly IMvxNavigationService _navigationService;
//        private readonly ILoginService _loginService;

//        private string _mobileNumber;
//        public string MobileNumber
//        {
//            get
//            {
//                return _mobileNumber;
//            }
//            set
//            {
//                _mobileNumber = value;
//                RaisePropertyChanged(() => MobileNumber);
//            }
//        }

//        private Int16? _firstNumber;
//        public Int16? FirstNumber
//        {
//            get
//            {
//                return _firstNumber;
//            }
//            set
//            {
//                if (value.HasValue && value.Value < 10 && value.Value >= 0)
//                {
//                    _firstNumber = value;
//                }

//                RaisePropertyChanged(() => FirstNumber);
//            }
//        }

//        private Int16? _secondNumber;
//        public Int16? SecondNumber
//        {
//            get
//            {
//                return _secondNumber;
//            }
//            set
//            {
//                if (value.HasValue && value.Value < 10 && value.Value >= 0)
//                {
//                    _secondNumber = value;
//                }
               
//                RaisePropertyChanged(() => SecondNumber);
//            }
//        }

//        private Int16? _thirdNumber;
//        public Int16? ThirdNumber
//        {
//            get
//            {
//                return _thirdNumber;
//            }
//            set
//            {
//                if (value.HasValue && value.Value < 10 && value.Value >= 0)
//                {
//                    _thirdNumber = value;
//                }
               
//                RaisePropertyChanged(() => ThirdNumber);
//            }
//        }

//        private Int16? _fourNumber;
//        public Int16? FourNumber
//        {
//            get
//            {
//                return _fourNumber;
//            }
//            set
//            {
//                if (value.HasValue && value.Value < 10 && value.Value >= 0)
//                {
//                    _fourNumber = value;
//                }
               
//                RaisePropertyChanged(() => FourNumber);
//            }
//        }

//        bool _isValidationVisible;
//        public bool IsValidationVisible
//        {
//            get
//            {
//                return _isValidationVisible;
//            }
//            set
//            {
//                _isValidationVisible = value;
//                RaisePropertyChanged(() => IsValidationVisible);
//            }
//        }

//        string  _errorMessage;
//        public string ErrorMessage
//        {
//            get
//            {
//                return _errorMessage;
//            }
//            set
//            {
//                _errorMessage = value;
//                RaisePropertyChanged(() => ErrorMessage);
//            }
//        }

//        UserDto _userDto;

//        public SignupStepCodeViewModel(IMvxNavigationService navigationService, ILoginService loginService)
//        {
//            _navigationService = navigationService;
//            _loginService = loginService;
//            GotoNamesPageCommand = new MvxAsyncCommand(GotoNamesPage);
//        }               

//        private async Task GotoNamesPage()
//        {
//            try
//            {
//                IsValidationVisible = false ;
//                ErrorMessage = "";

//                if (this.FirstNumber == null || this.SecondNumber == null || this.ThirdNumber == null || this.FourNumber == null)
//                {
//                    IsValidationVisible = true;
//                    ErrorMessage = StringResources.WrongCode;
//                    return;
//                }

//                this.IsBusy = true;

//                var code = Int16.Parse(this.FirstNumber.Value.ToString() + this.SecondNumber.Value.ToString() + this.ThirdNumber.Value.ToString() + this.FourNumber.Value.ToString());

//                await _loginService.VerifyCode(long.Parse(this.MobileNumber), code);

//                await _navigationService.Navigate<SignupStepDetailsViewModel, UserDto>(_userDto);
//                await _navigationService.Close(this);
//            }           
//            catch (System.Exception ex)
//            {
//                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoNamesPage");
//            }
//            finally
//            {
//                this.IsBusy = false;
//            }
//        }
//        public override void Prepare(UserDto parameter)
//        {
//            _userDto = parameter;
//            if (_userDto != null)
//            {
//                MobileNumber = "+" + _userDto.PhoneCountryCode.ToString() + _userDto.PhoneNumber.ToString();
//            }

//        }
//    }
//}
