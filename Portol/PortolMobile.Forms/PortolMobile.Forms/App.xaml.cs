using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PortolMobile.Forms.Views;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Login;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PortolMobile.Forms
{
    public partial class App : Application
    {
            
        public App()
        {
            InitializeComponent();
            ViewModelLocator.RegisterDependencies(false);
            InitNavigation();
        }

        private Task InitNavigation()
        {
            try
            {
                
                NavigationPage navigationPage = new NavigationPage();               
                navigationPage.BarBackgroundColor = Color.FromHex("#121010");              
                MainPage = navigationPage;
                var navigationService = ViewModelLocator.Resolve<INavigationService>();
                navigationService.SetNavigationPage(navigationPage);
                return navigationService.NavigateToAsync<LoginViewModel>();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
