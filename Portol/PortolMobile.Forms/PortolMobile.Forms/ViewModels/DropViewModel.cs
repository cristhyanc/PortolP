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

        public ICommand FindCustomerCommand { get; private set; }
        
        private IUserMobileService _customerService;

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

        public string SenderName
        {
            get { return string.Format(StringResources.HelloMessage, _sessionData.User.FirstName) ; }
          
        }

        ISessionData _sessionData;


        public DropViewModel(IUserMobileService customerService, INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData) : base(navigationService, userDialogs)
        {
            GetCustomerCommand = new Command((() => GotoAddressStep()), () => { return !IsBusy; });
            GotoShopCommand = new Command(GotoShop, () => { return !IsBusy; });
            FindCustomerCommand = new Command((() => FindCustomer()), () => { return !IsBusy; });
            _customerService = customerService;
            _sessionData = sessionData;
            //this.EmailMobileNumber = "0405593358";
            //this.ReceiverName = "Cris";

        }


        private async void GotoShop()
        {
            await NavigationService.NavigateToAsync<ShopViewModel>();
        }

        private async Task FindCustomer()
        {
            try
            {
                if (this.IsBusy)
                {
                    return;
                }

                if (string.IsNullOrEmpty(this.EmailMobileNumber))
                {
                    return;
                }

               this.UserDialogs.ShowLoading();

                CustomerDto customer = null;
                long number = 0;

                if (long.TryParse(this.EmailMobileNumber, out number))
                {
                    customer = await _customerService.GetCustomerByPhoneNumber(number, _sessionData.User.PhoneCountryCode);
                }

                if (customer == null)
                {
                    if (Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                    {
                        customer = await _customerService.GetCustomerByEmail(EmailMobileNumber);
                    }
                }

                if (customer!=null)
                {
                    this.ReceiverName = customer.FullName;
                }
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "FindCustomer");
            }
            finally
            {
                this.UserDialogs.HideLoading();
            }
        }

       
        public async Task GotoAddressStep()
        {
            try
            {
                this.IsBusy = true;

                if (!FieldsValidation())
                {
                    return;
                }

              
                CustomerDto customer = null;
                long number = 0;

                if (long.TryParse(this.EmailMobileNumber, out number))
                {
                    customer = await _customerService.GetCustomerByPhoneNumber(number, _sessionData.User.PhoneCountryCode);

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

                        customer.PhoneCountryCode = _sessionData.User.PhoneCountryCode;
                        customer.IsGuess = true;
                        customer.FirstName = this.ReceiverName;
                    }
                }

                DropoffDto dropoffDto = new DropoffDto();
                dropoffDto.Receiver = customer;
                dropoffDto.Sender = _sessionData.User;


                await NavigationService.NavigateToAsync<DropAddressViewModel>(dropoffDto);


            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "GotoAddressStep");
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
