//using Acr.UserDialogs;
//using MvvmCross.Commands;
//using MvvmCross.Logging;
//using MvvmCross.Navigation;
//using Portol.Common;
//using Portol.Common.DTO;
//using Portol.Common.Helper;
//using Portol.Common.Interfaces.PortolMobile;
//using Portol.DTO;
//using PortolMobile.Core.Helper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PortolMobile.Forms.ViewModels.SignUp
//{
//    public class SignupStepMobileViewModel : BaseViewModel
//    {
//        public IMvxCommand GotoCodePageCommand { get; private set; }
//        private readonly IMvxNavigationService _navigationService;
//        private readonly ILoginService _loginService;
//        public IMvxCommand SelectCountryCommand { get; private set; }

//        List<CountryDto> _countryItems;
//        public List<CountryDto> CountryItems
//        {
//            get
//            {
//                return _countryItems;
//            }
//            set
//            {
//                _countryItems = value;
//                RaisePropertyChanged(() => CountryItems);
//            }
//        }

//        CountryDto _countrySelected;
//        public CountryDto CountrySelected
//        {
//            get
//            {
//                return _countrySelected;
//            }
//            set
//            {
//                _countrySelected = value;
//                RaisePropertyChanged(() => CountrySelected);
//            }
//        }

//        bool _isMobileValidationVisible;
//        public bool IsMobileValidationVisible
//        {
//            get
//            {
//                return _isMobileValidationVisible;
//            }
//            set
//            {
//                _isMobileValidationVisible = value;
//                RaisePropertyChanged(() => IsMobileValidationVisible);
//            }
//        }


//        string _validationMessage;
//        public string ValidationMessage
//        {
//            get
//            {
//                return _validationMessage;
//            }
//            set
//            {
//                _validationMessage = value;
//                RaisePropertyChanged(() => ValidationMessage);
//            }
//        }
        

//        private string _mobileNumber;
//        public string MobileNumber
//        {
//            get
//            {
//                return _mobileNumber;
//            }
//            set
//            {
//                long num = 0;
//                if (long.TryParse(value, out num))
//                {
//                    _mobileNumber = value;
//                }
//                else
//                {
//                    _mobileNumber = "";
//                }

//                RaisePropertyChanged(() => MobileNumber);
//            }
//        }

//        public SignupStepMobileViewModel(IMvxNavigationService navigationService, ILoginService loginService)
//        {
//            _navigationService = navigationService;
//            _loginService = loginService;
//            CountryItems = new List<CountryDto>(Constants.CountryList);
//            CountrySelected = CountryItems.Where(x => x.Country == EnumCountries.Australia).FirstOrDefault();
//            GotoCodePageCommand = new MvxAsyncCommand(GotoCodePage);
//            SelectCountryCommand = new MvxCommand(OpenCountryList);
//        }

//        private void OpenCountryList()
//        {
//            try
//            {
//                var cfg = new ActionSheetConfig()
//                   .SetTitle(StringResources.Countries);
//                foreach (var item in CountryItems)
//                {
//                    cfg.Add(
//                       item.CountryName,
//                        () => {
//                            this.CountrySelected = item;
//                        },
//                       item.CountryFlagFile
//                        );
//                }

//                cfg.SetCancel(null);
//                var disp = this.UserDialogs.ActionSheet(cfg);
//            }
//            catch (System.Exception ex)
//            {
//                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.RecoveringPassword, "SaveNewPassword");
//            }
//        }

//        private async Task GotoCodePage()
//        {
//            try
//            {
//                IsMobileValidationVisible = false;
//                if (string.IsNullOrEmpty(MobileNumber))
//                {
//                    IsMobileValidationVisible = true;
//                    ValidationMessage = StringResources.MobileNumberRequiered;
//                    return;
//                }

//                this.IsBusy = true;
//                UserDto user = new UserDto();
//                user.PhoneCountryCode = int.Parse(this.CountrySelected.CountryCode);
//                user.PhoneNumber = long.Parse(MobileNumber);

//               var isUniqui= await _loginService.VerifyMobileUniqueness(user.PhoneNumber, user.PhoneCountryCode);

//                if(!isUniqui)
//                {
//                    IsMobileValidationVisible = true;
//                    ValidationMessage = StringResources.MobileInUse;
//                    return;
//                }

//                var result = await _loginService.SendVerificationCode(user.PhoneNumber,user.PhoneCountryCode);
//                if(result)
//                {
//                    await _navigationService.Navigate<SignupStepCodeViewModel, UserDto>(user);
//                    await _navigationService.Close(this);
//                }               
//            }          
//            catch (System.Exception ex)
//            {
//                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoCodePage");               
//            }
//            finally
//            {
//                this.IsBusy = false;
//            }
//        }
//    }
//}
