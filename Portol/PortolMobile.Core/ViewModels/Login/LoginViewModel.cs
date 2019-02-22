using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;

namespace PortolMobile.Core.ViewModels.Login
{
   public class LoginViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;
        
             public IMvxCommand LoginButtonCommand { get; private set; }
        private string  _emailText;
        public string EmailText
        {
            get
            {
                return _emailText;
            }
            set
            {
                _emailText = value;
                RaisePropertyChanged(() => EmailText);
            }
        }

        public LoginViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            EmailText = "cristhyan@systar.com";
            LoginButtonCommand = new MvxAsyncCommand(LoginUser);
        }

        private async Task LoginUser()
        {
           await  _navigationService.Navigate<FirstPageViewModel>();
        }
    }
}
