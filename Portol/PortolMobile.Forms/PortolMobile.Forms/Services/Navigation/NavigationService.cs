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

namespace PortolMobile.Forms.Services.Navigation
{
  public  class NavigationService : INavigationService
    {
     //   AppSessionSettings _appSettings;
        NavigationPage CurrentNavigator;
        public NavigationService()
        {
          //  _appSettings = ViewModelLocator.Resolve<AppSessionSettings>();
        }

        private bool _popupOn = false;

        //public async Task OpenPopup(BaseViewModel viewModel, object parameter = null)
        //{
        //    _popupOn = true;
        //    PopupPage page = (PopupPage)CreatePage(viewModel.GetType(), null);
        //    page.BindingContext = viewModel;
        //    await viewModel.InitializeAsync(parameter);
        //    if (CurrentNavigator != null && CurrentNavigator.Navigation != null)
        //    {
        //        await CurrentNavigator.Navigation.PushPopupAsync(page);
        //    }
        //}

        //public async Task OpenPopup(PopupPage page)
        //{
        //    _popupOn = true;
        //    if (CurrentNavigator != null && CurrentNavigator.Navigation != null && page != null)
        //    {
        //        await CurrentNavigator.Navigation.PushPopupAsync(page);
        //    }
        //}

        //public async Task ClosePopup()
        //{
        //    if (_popupOn)
        //    {
        //        await CurrentNavigator.Navigation.PopAllPopupAsync();
        //    }
        //    _popupOn = false;
        //}

        public void SetNavigationPage(NavigationPage navigation)
        {
            CurrentNavigator = navigation;
            CurrentNavigator.ChildRemoved += MainNavigator_ChildRemoved;
        }

        private async void MainNavigator_ChildRemoved(object sender, ElementEventArgs e)
        {
            //if (sender != null)
            //{
            //    Page page = (Page)sender;
            //    try
            //    {

            //        if (page.BindingContext != null)
            //        {
            //            await (page.BindingContext as BaseViewModel).Destroy();
            //            page.BindingContext = null;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
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
            }

        }

        public async Task GoToMainPage()
        {

            if (CurrentNavigator != null)
            {
                await CurrentNavigator.Navigation.PopToRootAsync();
            }
        }

        public async Task RemoveCurrentPage()
        {
            if (CurrentNavigator != null)
            {
                await CurrentNavigator.Navigation.PopAsync();
            }
        }

        public void Logout()
        {
            //_appSettings.User.KeepLoggedIn = false;
            //IUserBl _userBl = ViewModelLocator.Resolve<IUserBl>();
            //_userBl.SaveUserDetails(_appSettings.User);
            //var page = new LoadingMetaDataView();
            //CurrentNavigator = null;
            //Application.Current.MainPage = page;
            //page.BindingContext = new LoadingMetaDataViewModel(true);
            //GC.Collect();
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, BaseViewModel viewModel = null)
        {
            try
            {
                Page page = CreatePage(viewModelType, parameter);

                if(CurrentNavigator!=null)
                {
                    await CurrentNavigator.PushAsync(page);
                    //if (Application.Current.MainPage == null)
                    //{
                    //    Application.Current.MainPage = page;
                    //}
                    //else
                    //{
                       
                    //}
                }
                else
                {
                    Application.Current.MainPage = page;
                }
              






                //if (page is LoginView )
                //{
                //    CurrentNavigator = null;
                //    Application.Current.MainPage = page;
                //}
                //else
                //{
                //    if (CurrentNavigator != null)
                //    {
                //        //if (_popupOn)
                //        //{
                //        //    await ClosePopup();
                //        //}

                //        await CurrentNavigator.PushAsync(page);
                //    //    ((MainView)Application.Current.MainPage).IsPresented = false;


                //    }
                //    else
                //    {

                //        if (typeof(MainView) == page.GetType())
                //        {
                //            Application.Current.MainPage = page;
                //            Application.Current.MainPage.BindingContext = new MainViewModel();
                //            page = Application.Current.MainPage;
                //        }
                //        else
                //        {
                //            Application.Current.MainPage = null;
                //            //TODO
                //            //Application.Current.MainPage = new MainView(page)
                //            //{
                //            //    BindingContext = new MainViewModel()
                //            //};
                //        }
                //    }
                //}

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

        public Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        public ContentPage CreateContentPage(Type viewModelType, object parameter)
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