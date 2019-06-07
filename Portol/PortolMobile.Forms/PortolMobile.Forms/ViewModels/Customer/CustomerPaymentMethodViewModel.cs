using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Customer
{
   public class CustomerPaymentMethodViewModel: BaseViewModel
    {
        ISessionData _sessionData;
        IPaymentService _paymentService;

        private PaymentMethodDto _paymentMethod;
        public PaymentMethodDto PaymentMethod
        {
            get
            {
                return _paymentMethod;
            }
            set
            {
                _paymentMethod = value;
                OnPropertyChanged();
            }
        }

        private string _requiredLabel;
        public string RequiredLabel
        {
            get
            {
                return _requiredLabel;
            }
            set
            {
                _requiredLabel = value;
                OnPropertyChanged();
            }
        }

        private string _month;
        public string Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
                if (!string.IsNullOrEmpty(_month))
                {
                    int month = 0;
                    int.TryParse(_month, out month);
                    PaymentMethod.ExpMonth  = month;
                }
                OnPropertyChanged();
            }
        }

        private string _year;
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                if(!string.IsNullOrEmpty(_year))
                {
                    int year = 0;
                    int.TryParse(_year, out year);
                    PaymentMethod.ExpYear = year;
                }
                OnPropertyChanged();
            }
        }

        public ICommand AddNewPaymentCommand { get; private set; }

        IUserCore _userCore;

        public CustomerPaymentMethodViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IPaymentService paymentService, IUserCore userCore ) : base(navigationService, userDialogs)
        {
            AddNewPaymentCommand = new Command(CreateCreditCard, () => { return !IsBusy; });
            _sessionData = sessionData;
            _paymentService = paymentService;
            PaymentMethod = new PaymentMethodDto();
            _userCore = userCore;
           
        }

        private async void CreateCreditCard()
        {
            try
            {
                this.IsBusy = true;
                RequiredLabel = "";
                if (string.IsNullOrEmpty(PaymentMethod.CardNumber))
                {
                    RequiredLabel = StringResources.CreditCardNumberRequired;
                    return;
                }

                if(PaymentMethod.ExpMonth<1 || PaymentMethod.ExpMonth>12)
                {
                    RequiredLabel = StringResources.ExpiryMonthRequired;
                    return;
                }

                if ((PaymentMethod.ExpYear +2000) < DateTime.Now.Year )
                {
                    RequiredLabel = StringResources.ExpiryYearRequired;
                    return;
                }

                if (string.IsNullOrEmpty(PaymentMethod.CVV))
                {
                    RequiredLabel = StringResources.MissingInformation;
                    return;
                }

                if (string.IsNullOrEmpty(_sessionData.User.CustomerPaymentID))
                {
                    _sessionData.User.CustomerPaymentID = await _paymentService.CreateCustomer(_sessionData.User);
                    if (string.IsNullOrEmpty(_sessionData.User.CustomerPaymentID) || !await _userCore.SaveCustomer(_sessionData.User))
                    {
                        UserDialogs.Alert(StringResources.ProblemTryAgain);
                        return;
                    }
                }
                PaymentMethod.CardServiceID = await _paymentService.LinkNewCreditCard(_sessionData.User.CustomerPaymentID, PaymentMethod);

                if (!string.IsNullOrEmpty(PaymentMethod.CardServiceID))
                {
                    PaymentMethod.CustomerID = _sessionData.User.CustomerID;
                    await _userCore.SavePaymentMethod(PaymentMethod);
                    await this.NavigationService.GoToPreviousPageAsync();
                }
                else
                {
                    UserDialogs.Alert(StringResources.ProblemTryAgain);
                    return;
                }

            }
            catch (Exception ex)
            {

                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodViewModel", "CreateCreditCard");
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
                if (navigationData != null)
                {
                    PaymentMethod = (PaymentMethodDto)navigationData;
                }
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
