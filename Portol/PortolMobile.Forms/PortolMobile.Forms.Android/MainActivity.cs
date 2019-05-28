using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using FFImageLoading;
using System;
using Android.Util;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace PortolMobile.Forms.Droid
{


    [Activity(Label = "Portol", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
       App currentApp;
        private string tag = "Portol";
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);
                Xamarin.Essentials.Platform.Init(this, bundle); 

                ImageCircleRenderer.Init();
                global::Xamarin.Forms.Forms.Init(this, bundle);
           
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
                AppCenter.Start("952146a6-94b4-4717-9d75-546346e67f3a", typeof(Push),typeof(Analytics), typeof(Crashes));


                var config = new FFImageLoading.Config.Configuration()
                {
                    VerboseLogging = true,
                    VerbosePerformanceLogging = true,
                    VerboseMemoryCacheLogging = true,
                    VerboseLoadingCancelledLogging = true,
                    Logger = new CustomLogger(),
                };

                ImageService.Instance.Initialize(config);

                UserDialogs.Init(this);

                currentApp = new App();
                LoadApplication(currentApp);
                App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            }
            catch (Exception ex)
            {
                Log.Error(tag, ex.Message);
                throw ex;
            }

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

    public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
    {
        public void Debug(string message)
        {
          //  Console.WriteLine(message);
        }

        public void Error(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        public void Error(string errorMessage, Exception ex)
        {
            Error(errorMessage + System.Environment.NewLine + ex.ToString());
        }
    }
}

