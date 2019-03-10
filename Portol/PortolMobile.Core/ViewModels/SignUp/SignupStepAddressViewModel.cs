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
    public class SignupStepAddressViewModel : BaseViewModel<UserDto>
    {

        public IMvxCommand SaveAccountCommand { get; private set; }
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

        private string _unitNumber;
        public string UnitNumber
        {
            get
            {
                return _unitNumber;
            }
            set
            {
                _unitNumber = value;
                RaisePropertyChanged(() => UnitNumber);
            }
        }

        private string _street;
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
                RaisePropertyChanged(() => Street);
            }
        }

        private string _suburb;
        public string Suburb
        {
            get
            {
                return _suburb;
            }
            set
            {
                _suburb = value;
                RaisePropertyChanged(() => Suburb);
            }
        }

        private string _postCode;
        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                _postCode = value;
                RaisePropertyChanged(() => PostCode);
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        private string _country;
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                RaisePropertyChanged(() => Country);
            }
        }

        public SignupStepAddressViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            SaveAccountCommand = new MvxAsyncCommand(SaveNewAccount);
        }

        private async Task SaveNewAccount()
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.SignUp, "SaveNewAccount");
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
