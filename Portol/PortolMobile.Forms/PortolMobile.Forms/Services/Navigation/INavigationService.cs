using PortolMobile.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortolMobile.Forms.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;

        Task GoToMainPage();

        Task RemoveCurrentPage();

        Page CreatePage(Type viewModelType, object parameter);

        Task GoToPreviousPageAsync();

        ContentPage CreateContentPage(Type viewModelType, object parameter);

       // Task OpenPopup(BaseViewModel viewModel, object parameter = null);

       // Task OpenPopup(PopupPage page);

      //  Task ClosePopup();

        void SetNavigationPage(NavigationPage navigation);

        void Logout();
    }
}
