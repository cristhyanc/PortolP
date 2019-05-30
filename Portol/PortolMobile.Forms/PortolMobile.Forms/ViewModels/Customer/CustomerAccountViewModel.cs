using Acr.UserDialogs;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms.ViewModels.Customer
{
    public class CustomerAccountViewModel: BaseViewModel
    {
        string _userAddress;
        public string UserAddress
        {
            get { return _userAddress; }
            set
            {
                _userAddress = value;
                OnPropertyChanged();
            }
        }


        ISessionData _sessionData;
        public CustomerAccountViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData) : base(navigationService, userDialogs)
        {          
            _sessionData = sessionData;           
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                if(_sessionData?.User?.CustomerAddress!=null)
                {
                    UserAddress = _sessionData.User.CustomerAddress.FullAddress;
                }
                
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
