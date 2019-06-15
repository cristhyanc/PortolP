using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
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

        
        public ICommand DeleteCreditCardCommand { get; private set; }
        public ICommand AddNewPaymentCommand { get; private set; }
        ISessionData _sessionData;
        IPaymentService _paymentService;
        IUserCore _userCore;
        public CustomerPaymentMethodsViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IPaymentService paymentService, IUserCore userCore) : base(navigationService, userDialogs)
        {
            AddNewPaymentCommand = new Command((() => GotoNewPayment()), () => { return !IsBusy; });
            DeleteCreditCardCommand = new Command<Guid>(((Guid id) => DeleteCreditCard(id)), (Guid id) => { return !IsBusy; });
            _sessionData = sessionData;
            _paymentService = paymentService;
            _userCore = userCore;
        }


        private async Task  DeleteCreditCard(Guid paymentMethodId)
        {
            try
            {
                if(!(await this.UserDialogs.ConfirmAsync(StringResources.DeletePaymentMethodQuestion)))
                {
                    return;
                }
                this.IsBusy = true;
                var payment = this.PaymentMethods.Where(x => x.PaymentMethodID == paymentMethodId).FirstOrDefault();
                var result = await _paymentService.DeleteCreditCard(_sessionData.User.CustomerPaymentID, payment.CardServiceID);

                if(result )
                {
                   await _userCore.DeletePaymentMethodByServiceID(payment.CardServiceID);
                }

                PageAppearing();
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodsViewModel", "DeleteCreditCard");
            }
           
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


        protected override async void PageAppearing()
        {
            try
            {
                this.IsBusy = true;
                this.PaymentMethods = await _paymentService.GetCustomerPaymentMethods(_sessionData.User.CustomerPaymentID);
                //if (!string.IsNullOrEmpty(_sessionData.User.CustomerPaymentID))
                //{
                //    this.PaymentMethods  = new List<PaymentMethodDto>();
                //   var serviceMethods = await _paymentService.GetCustomerPaymentMethods(_sessionData.User.CustomerPaymentID);

                //    if(serviceMethods?.Count>0 && _sessionData.User.PaymentMethods?.Count>0)
                //    {                      
                //        PaymentMethodDto method;
                //        _sessionData.User.PaymentMethods.ForEach((x) => {

                //            var userPayment = serviceMethods.Where(y => y.CardServiceID.Equals(x.CardServiceID)).FirstOrDefault();
                //            if(userPayment !=null)
                //            {
                //                method = userPayment;
                //                method.PaymentMethodID = x.PaymentMethodID;
                //                _paymentMethods.Add(method);
                //            }

                //            OnPropertyChanged("PaymentMethods");
                //        });
                //    }
                //}
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
