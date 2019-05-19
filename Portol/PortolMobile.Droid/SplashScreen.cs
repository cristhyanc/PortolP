using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Cross;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace PortolMobile.Droid
{
    [MvxActivityPresentation]
    [Activity(
        MainLauncher = true,
        Icon = "@mipmap/icon",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        //protected  override void OnCreate(Bundle bundle)
        //{
            //try
            //{
            //    base.OnCreate(bundle);

            //    var _imageView = FindViewById<MvxSvgCachedImageView>(Resource.Id.svgImage);

            //    ImageService.Instance
            //           .LoadFileFromApplicationBundle("disabled.svg")
            //           .WithCustomDataResolver(new FFImageLoading.Svg.Platform.SvgDataResolver(200, 0, true))
            //           .WithCustomLoadingPlaceholderDataResolver(new FFImageLoading.Svg.Platform.SvgDataResolver(200, 0, true))
            //           .Into(_imageView);
            //}
            //catch (System.Exception ex)
            //{

            //    throw ex;
            //}
           
            //  ParentActivity.SupportActionBar.Title = StringResc.Login;

           
        //}
    }
}