using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Views.InputMethods;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.ViewModels.SignUp;

namespace PortolMobile.Droid.Views.Signup
{
    [MvxActivityPresentation]
    [Activity(Label = "Portol",
     Theme = "@style/AppTheme",
     LaunchMode = LaunchMode.SingleTop,
     Name = "PortolMobile.Droid.Views.Signup.SignupStepDetailsView"
     )]
    public class SignupStepDetailsView : MvxAppCompatActivity<SignupStepDetailsViewModel>
    {
        public DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            SetContentView(Resource.Layout.Signup_Step_DetailsView);

        }


        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
                return;

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }
    }



}
