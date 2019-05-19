using Acr.UserDialogs;
using Portol.Common.DTO;
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
        public ICommand AddNewPaymentCommand { get; private set; }        
        CustomerDto customer;

        public CustomerPaymentMethodsViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            AddNewPaymentCommand = new Command((() => GotoNewPayment()), () => { return !IsBusy; });
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

        public override Task InitializeAsync(object navigationData)
        {
            try
            {               
                customer = (CustomerDto)navigationData;               
            }
            catch (Exception ex)
            {
                this.IsBusy = false;             
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerPaymentMethodsViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
