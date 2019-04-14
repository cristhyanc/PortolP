using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using FFImageLoading;
using System;
using Android.Util;
using ImageCircle.Forms.Plugin.Droid;

namespace PortolMobile.Forms.Droid
{


    [Activity(Label = "Portol", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
       App currentApp;
        private string tag = "iSystain";
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);
                ImageCircleRenderer.Init();
                global::Xamarin.Forms.Forms.Init(this, bundle);
                //AnimationViewRenderer.Init();
                //PlotViewRenderer.Init();
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            //   CachedImageRenderer.Init(false);
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

