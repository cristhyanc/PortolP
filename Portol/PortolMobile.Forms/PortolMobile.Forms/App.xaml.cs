using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PortolMobile.Forms.Views;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Login;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PortolMobile.Forms.Controls;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PortolMobile.Forms
{
    public partial class App : Application
    {
            
        public App()
        {
            InitializeComponent();
            AppCenter.Start("android=952146a6-94b4-4717-9d75-546346e67f3a;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}", typeof(Push), typeof(Analytics), typeof(Crashes));

            ViewModelLocator.RegisterDependencies(false);
            InitNavigation();
        }

        private Task InitNavigation()
        {
            try
            {

                CustomNavigationPage navigationPage = new CustomNavigationPage();               
              //  navigationPage.BarBackgroundColor = Color.FromHex("#121010");              
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
