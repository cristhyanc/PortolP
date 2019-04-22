using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class NavigationServiceMK : INavigationService
    {
        public Type viewModel;
        public object Parameter;
        public ContentPage CreateContentPage(Type viewModelType, object parameter)
        {
            throw new NotImplementedException();
        }

        public Page CreatePage(Type viewModelType, object parameter)
        {
            throw new NotImplementedException();
        }

        public Task GoToMainPage()
        {
            throw new NotImplementedException();
        }

        public Task GoToPreviousPageAsync()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

       
        public Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            viewModel = typeof(TViewModel);
            return null;
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            viewModel = typeof(TViewModel);
            Parameter = parameter;
            return Task.Run(()=> { });
        }

        public Task RemoveCurrentPage()
        {
            throw new NotImplementedException();
        }

        public void SetNavigationPage(NavigationPage navigation)
        {
            throw new NotImplementedException();
        }
    }
}
