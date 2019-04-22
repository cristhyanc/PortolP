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

        string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
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

                OnPropertyChanged();
            }
        }

        public DropViewModel(ICustomerMobileService customerService, INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
            GetCustomerCommand = new Command((() => GetCustomer()));
            GotoShopCommand = new Command(GotoShop);
            _customerService = customerService;
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

                long.TryParse(this.MobileNumber, out number);

                customer = await _customerService.GetCustomerByPhoneNumber(number, SessionData.User.PhoneCountryCode);
                               

                if (customer == null)
                {
                    if (!string.IsNullOrEmpty(this.Email))
                    {
                        if (await this.DisplayMessageQuestion(StringResources.Guess, StringResources.NoMobileNumberEmail, StringResources.Yes, StringResources.No))
                        {
                            customer = await _customerService.GetCustomerByEmail(Email);
                        }
                    }

                    if (customer == null)
                    {
                        if (!await this.DisplayMessageQuestion(StringResources.Guess, StringResources.PersonNoRegistered, StringResources.ContinueGuess))
                        {
                            return;
                        }

                        customer = new CustomerDto();
                        customer.Email = this.Email;
                        if (string.IsNullOrEmpty(this.Email))
                        {
                            customer.Email = StringResources.GuessEmail;
                        }
                        customer.PhoneNumber = number;
                        customer.PhoneCountryCode = SessionData.User.PhoneCountryCode;
                        customer.IsGuess = true;
                        customer.FirstName = this.ReceiverName;
                    }
                }

                //this.ReceiverName = customer.FirstName;
                //string message = StringResources.ReceiverName + ": " + this.ReceiverName + Environment.NewLine;
                //message += StringResources.MobileNumber + ": " + customer.PhoneNumber.ToString() + Environment.NewLine;
                //message += StringResources.Email + ": " + customer.Email;

                //if (await this.DisplayMessageQuestion(StringResources.ReceiverInformation, message, StringResources.Continue))
                //{
                await NavigationService.NavigateToAsync<DropAddressViewModel>(customer);
                //}

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
            if (string.IsNullOrEmpty(this.MobileNumber))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberRequiered);
                return false;
            }
            long number = 0;
            if (!long.TryParse(this.MobileNumber, out number))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberRequiered);
                return false;
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
