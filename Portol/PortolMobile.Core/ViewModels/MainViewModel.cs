using MvvmCross.Commands;
using MvvmCross.Navigation;
using PortolMobile.Core.ViewModels.Login;

namespace PortolMobile.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            //ShowPeopleViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<PeopleViewModel>());
            ShowLoginPageViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<LoginViewModel>());
            ShowMenuViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MenuViewModel>());
        }

        // MvvmCross Lifecycle

        // MVVM Properties

        // MVVM Commands
        public IMvxAsyncCommand ShowLoginPageViewModelCommand { get; private set; }
       // public IMvxAsyncCommand ShowPlanetsViewModelCommand { get; private set; }
        public IMvxAsyncCommand ShowMenuViewModelCommand { get; private set; }

        // Private methods
    }
}
