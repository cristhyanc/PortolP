﻿using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using Portol.DTO;
using PortolMobile.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.ViewModels.SignUp
{
    public class SignupStepMobileViewModel : BaseViewModel
    {
        public IMvxCommand GotoCodePageCommand { get; private set; }
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        List<CountryDto> _countryItems;
        public List<CountryDto> CountryItems
        {
            get
            {
                return _countryItems;
            }
            set
            {
                _countryItems = value;
                RaisePropertyChanged(() => CountryItems);
            }
        }

        CountryDto _countrySelected;
        public CountryDto CountrySelected
        {
            get
            {
                return _countrySelected;
            }
            set
            {
                _countrySelected = value;
                RaisePropertyChanged(() => CountrySelected);
            }
        }

        bool _isMobileValidationVisible;
        public bool IsMobileValidationVisible
        {
            get
            {
                return _isMobileValidationVisible;
            }
            set
            {
                _isMobileValidationVisible = value;
                RaisePropertyChanged(() => IsMobileValidationVisible);
            }
        }

        


        private string _mobileNumber;
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                long num = 0;
                if (long.TryParse(value, out num))
                {
                    _mobileNumber = value;
                }
                else
                {
                    _mobileNumber = "";
                }

                RaisePropertyChanged(() => MobileNumber);
            }
        }

        public SignupStepMobileViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            CountryItems = new List<CountryDto>(Constants.CountryList);
            CountrySelected = CountryItems.Where(x => x.Country == EnumCountries.Australia).FirstOrDefault();
            GotoCodePageCommand = new MvxAsyncCommand(GotoCodePage);
           
        }

        private async Task GotoCodePage()
        {
            try
            {
                IsMobileValidationVisible = false;
                if (string.IsNullOrEmpty(MobileNumber))
                {
                    IsMobileValidationVisible = true;
                    return;
                }

                this.IsBusy = true;
                UserDto user = new UserDto();
                user.PhoneCountryCode = int.Parse(this.CountrySelected.CountryCode);
                user.PhoneNumber = long.Parse(MobileNumber);
                var result = await _loginService.SendVerificationCode(user.PhoneNumber,user.PhoneCountryCode);
                if(result)
                {
                    await _navigationService.Navigate<SignupStepCodeViewModel, UserDto>(user);
                }               
            }          
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "GotoCodePage");               
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}