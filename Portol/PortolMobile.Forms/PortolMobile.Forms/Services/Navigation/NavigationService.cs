using PortolMobile.Forms.Controls;
using PortolMobile.Forms.ViewModels;
using PortolMobile.Forms.Views;
using PortolMobile.Forms.Views.Login;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace PortolMobile.Forms.Services.Navigation
{
  public  class NavigationService : INavigationService
    {

        CustomNavigationPage CurrentNavigator;
        private Page _dropOffPage;
        private Page _shopPage;
        public NavigationService()
        {
          
        }


        public void SetNavigationPage(CustomNavigationPage navigation)
        {
            CurrentNavigator = navigation;
          //  CurrentNavigator.Navigation..Appearing += CurrentNavigator_Appearing;


        }

        void CurrentNavigator_Appearing(object sender, EventArgs e)
        {
            SetNavigationColor();
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public async Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter);
        }


        public async Task GoToPreviousPageAsync()
        {

            if (CurrentNavigator != null)
            {
                await CurrentNavigator.Navigation.PopAsync();
                SetNavigationColor();
            }

        }

        public async Task GoToMainPage()
        {

            if (CurrentNavigator != null)
            {
                await CurrentNavigator.Navigation.PopToRootAsync();
                SetNavigationColor();
            }
        }

        public async Task RemoveCurrentPage()
        {
            if (CurrentNavigator != null)
            {
                await CurrentNavigator.Navigation.PopAsync();
               SetNavigationColor();
            }
        }

        private void SetNavigationColor()
        {
            if (CurrentNavigator?.CurrentPage is ExtendedContentPage contentPage)
            {
                
                if (contentPage.IsTextBarWhite)
                {
                    CurrentNavigator.BarTextColor = Color.White;

                }
                else
                {
                    CurrentNavigator.BarTextColor = Color.Black;
                }
            }
        }

        public async Task Logout()
        {
            _dropOffPage = null;
            _shopPage = null;
            CurrentNavigator = null;

            await InternalNavigateToAsync(typeof(ViewModels.Login.LoginViewModel), null);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, BaseViewModel viewModel = null)
        {
            try
            {

                Page page=null;
                if (typeof(DropViewModel) == viewModelType || typeof(ShopViewModel) == viewModelType)
                {

                    if (typeof(DropViewModel) == viewModelType)
                    {
                        if (_dropOffPage == null)
                        {
                            _dropOffPage = CreatePage(viewModelType);
                        }
                        page = _dropOffPage;
                    }

                    if (typeof(ShopViewModel) == viewModelType)
                    {
                        if (_shopPage == null)
                        {
                            _shopPage = CreatePage(viewModelType);
                        }
                        page = _shopPage;
                    }

                    if(CurrentNavigator!=null)
                    {
                        CurrentNavigator.Appearing -= CurrentNavigator_Appearing;
                    }

                    CurrentNavigator = new CustomNavigationPage(page);

                    CurrentNavigator.Appearing += CurrentNavigator_Appearing;

                    Application.Current.MainPage = CurrentNavigator;

                }
                else
                {
                    page = CreatePage(viewModelType);
                    if (CurrentNavigator == null)
                    {
                        CurrentNavigator = new CustomNavigationPage(page);
                        CurrentNavigator.Appearing += CurrentNavigator_Appearing;
                        Application.Current.MainPage = CurrentNavigator;
                    }
                    else
                    {
                        await CurrentNavigator.PushAsync(page);
                    }

                }

                SetNavigationColor();

                if (viewModel != null)
                {
                    page.BindingContext = viewModel;

                   
                }

                if (page.BindingContext != null)
                {
                    await (page.BindingContext as BaseViewModel).InitializeAsync(parameter);
                }

            }
            catch (Exception ex)
            {
                var stt = ex.Message;
                throw ex;
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        public Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        public ContentPage CreateContentPage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            ContentPage page = Activator.CreateInstance(pageType) as ContentPage;
            return page;
        }
    }
}