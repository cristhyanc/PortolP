using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Dropoff;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels
{
    public class DropViewModel : BaseViewModel
    {
        public ICommand GetCustomerCommand { get; private set; }
        public ICommand GotoShopCommand { get; private set; }
        private ICustomerMobileService _customerService;

        string _emailMobileNumber;
        public string EmailMobileNumber
        {
            get { return _emailMobileNumber; }
            set
            {
                _emailMobileNumber = value;
                OnPropertyChanged();
            }
        }

        string _receiverName;
        public string ReceiverName
        {
            get { return _receiverName; }
            set
            {
                _receiverName = value;
                OnPropertyChanged();
            }
        }

      

        public DropViewModel(ICustomerMobileService customerService, INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
            GetCustomerCommand = new Command((() => GetCustomer()));
            GotoShopCommand = new Command(GotoShop);
            _customerService = customerService;
            //this.EmailMobileNumber = "0405593358";
            //this.ReceiverName = "asd";
        }

        private async void GotoShop()
        {
            await NavigationService.NavigateToAsync<ShopViewModel>();
        }

        public async Task GetCustomer()
        {
            try
            {
                if (this.IsBusy)
                {
                    return;
                }

                if (!FieldsValidation())
                {
                    return;
                }

                this.IsBusy = true;
                CustomerDto customer = null;
                long number = 0;

                if (long.TryParse(this.EmailMobileNumber, out number))
                {
                    customer = await _customerService.GetCustomerByPhoneNumber(number, SessionData.User.PhoneCountryCode);
                    
                }              
                               

                if (customer == null)
                {
                    if (Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                    {
                        customer = await _customerService.GetCustomerByEmail(EmailMobileNumber);
                    }
                    
                    if (customer == null)
                    {
                        if (!await this.DisplayMessageQuestion(StringResources.Guess, StringResources.PersonNoRegistered, StringResources.ContinueGuess))
                        {
                            return;
                        }

                        customer = new CustomerDto();
                        customer.PhoneNumber = number;

                        customer.Email = StringResources.GuessEmail;
                        if (Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                        {
                            customer.Email = this.EmailMobileNumber;
                        }
                                                    
                        customer.PhoneCountryCode = SessionData.User.PhoneCountryCode;
                        customer.IsGuess = true;
                        customer.FirstName = this.ReceiverName;
                    }
                }
                               
                await NavigationService.NavigateToAsync<DropAddressViewModel>(customer);
              

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "GetCustomer");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private bool FieldsValidation()
        {
            if (string.IsNullOrEmpty(this.EmailMobileNumber))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberEmailRequiered);
                return false;
            }

            long number = 0;
            if (!long.TryParse(this.EmailMobileNumber, out number))
            {
               

                if (!Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberEmailRequiered);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(this.ReceiverName))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.ReceiverNameRequired);
                return false;
            }

            return true;
        }
    }
}
