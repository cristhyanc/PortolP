using Acr.UserDialogs;
using Portol.Common.DTO;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Customer
{
    public class CustomerAccountViewModel: BaseViewModel
    {

        
        public ICommand EditCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        
        string _userAddress;
        public string UserAddress
        {
            get {
                if (User?.CustomerAddress != null)
                {
                return     User.CustomerAddress.FullAddress;
                }
                return "";
            }
           
        }

        CustomerDto _user;
        public CustomerDto User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
                OnPropertyChanged("UserAddress");
            }
        }


        ISessionData _sessionData;
        public CustomerAccountViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData) : base(navigationService, userDialogs)
        {          
            _sessionData = sessionData;
            EditCommand = new Command((() => GoToEditPage()), () => { return !IsBusy; });
            LogoutCommand = new Command((() => Logout()), () => { return !IsBusy; });
        }

        private async Task Logout()
        {
            try
            {
                _sessionData.LogoutUser();
                await this.NavigationService.NavigateToAsync<LoginViewModel>();
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountViewModel", "PageAppearing");
            }
        }

        protected override void PageAppearing()
        {
            try
            {
                User = _sessionData.User;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountViewModel", "PageAppearing");
            }
        }

        private async void GoToEditPage()
        {
            try
            {
                await this.NavigationService.NavigateToAsync<CustomerAccountDetailViewModel>();
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountViewModel", "GoToEditPage");
            }
        }
       
    }
}
