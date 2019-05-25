using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Interfaces;
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
    public class CustomerPaymentMethodsViewModel : BaseViewModel
    {
        private List<PaymentMethodDto> _paymentMethods;
        public List<PaymentMethodDto> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            set
            {
                _paymentMethods = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNewPaymentCommand { get; private set; }
        ISessionData _sessionData;
        IPaymentService _paymentService;
        public CustomerPaymentMethodsViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IPaymentService paymentService ) : base(navigationService, userDialogs)
        {
            AddNewPaymentCommand = new Command((() => GotoNewPayment()), () => { return !IsBusy; });
            _sessionData = sessionData;
            _paymentService = paymentService;
        }

        private async void GotoNewPayment()
        {
            try
            {
                await this.NavigationService.NavigateToAsync<CustomerPaymentMethodViewModel>();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodsViewModel", "GotoNewPayment");
            }
        }


        protected override async  void PageAppearing()
        {
            try
            {
                this.IsBusy = true;
                if (!string.IsNullOrEmpty(_sessionData.User.CustomerPaymentID))
                {
                    this.PaymentMethods = await _paymentService.GetCustomerPaymentMethods(_sessionData.User.CustomerPaymentID);
                }
            }
            catch (Exception ex)
            {

                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodsViewModel", "PageAppearing");
            }
            finally
            {
                this.IsBusy = false;
            }
        }       
    }
}
