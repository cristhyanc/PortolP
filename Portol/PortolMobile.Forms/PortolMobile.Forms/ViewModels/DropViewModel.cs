﻿using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Customer;
using PortolMobile.Forms.ViewModels.Dropoff;
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
        public ICommand GoToDeliveriesCommand { get; private set; }
        public ICommand GoToAccountCommand { get; private set; }

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

        string _bellIcon= "resource://PortolMobile.Forms.Resources.ic_bell.svg?assembly=PortolMobile.Forms";
        public string BellIcon
        {
            get { return _bellIcon; }
            set
            {
                _bellIcon = value;
                OnPropertyChanged();
            }
        }

        

        bool _isDeliveryOnItsWay;
        public bool IsDeliveryOnItsWay
        {
            get { return _isDeliveryOnItsWay; }
            set
            {
                _isDeliveryOnItsWay = value;
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
            get { return string.Format(StringResources.HelloMessage, _sessionData.User.FirstName); }

        }

        public string ProfilePicture
        {
            get { return _sessionData?.User?.ProfilePhoto?.ImageUrl; }

        }
        ISessionData _sessionData;
        IDeliveryCore _deliveryCore;

        public DropViewModel(IUserMobileService customerService, INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            try
            {
                GetCustomerCommand = new Command((() => GotoAddressStep()), () => { return !IsBusy; });
                GotoShopCommand = new Command(GotoShop, () => { return !IsBusy; });
                FindCustomerCommand = new Command((() => FindCustomer()), () => { return !IsBusy; });
                GoToAccountCommand = new Command((() => GoToAccount()), () => { return !IsBusy; });
                GoToDeliveriesCommand = new Command((() => GoToIncomingDeliveries()), () => { return !IsBusy; });
                _customerService = customerService;
                _sessionData = sessionData;
                _deliveryCore = deliveryCore;
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "DropViewModel");
            }

            this.EmailMobileNumber = "0405593357";
            this.ReceiverName = "Sophie";

        }

        protected override async void PageAppearing()
        {
            try
            {
                this.IsBusy = true;
                var delivery = await _deliveryCore.GetSendertDeliveryInProgress(_sessionData.User.CustomerID);
                if(delivery!=null)
                {                   
                    await NavigationService.NavigateToAsync<DropDriverInfoViewModel>(delivery);
                }

                BellIcon = "resource://PortolMobile.Forms.Resources.ic_bell.svg?assembly=PortolMobile.Forms";
                var incoming= await _deliveryCore.GetPendingReceiverDeliveries(_sessionData.User.CustomerID);
                if(incoming?.Count>0)
                {
                    BellIcon = "resource://PortolMobile.Forms.Resources.ic_bellRed.svg?assembly=PortolMobile.Forms";
                }

                
                OnPropertyChanged("ProfilePicture");
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "PageAppearing");
            }finally
            {
                this.IsBusy = false;
            }
        }

        private async void GoToAccount()
        {
            try
            {
                await NavigationService.NavigateToAsync<CustomerAccountViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "GoToAccount");
            }
        }

        private async void GoToIncomingDeliveries()
        {
            try
            {
                await NavigationService.NavigateToAsync<DropIncomingDeliveriesViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "GoToIncomingDeliveries");
            }
        }

        private async void GotoShop()
        {
          
            try
            {
                await NavigationService.NavigateToAsync<ShopViewModel>();

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "GoToIncomingDeliveries");
            }
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

                if (customer != null)
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
                }

                if (customer == null)
                {
                    this.UserDialogs.Alert(StringResources.PersonNoRegistered, StringResources.Validation);
                    return;
                }

                if (customer.CustomerID == _sessionData.User.CustomerID)
                {
                    this.EmailMobileNumber = "";
                    this.ReceiverName = "";
                    return;
                }

                DeliveryDto dropoffDto = new DeliveryDto();
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
