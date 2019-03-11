using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.ViewModels.Login ;

namespace PortolMobile.Droid.Views.Login
{
    [MvxActivityPresentation]
    [Activity(Label = "Portol",
     Theme = "@style/AppTheme",
     LaunchMode = LaunchMode.SingleTop,
     Name = "PortolMobile.Droid.Views.Signup.RecoverPasswordView"
     )]
    public class RecoverPasswordView : MvxAppCompatActivity<RecoverPasswordViewModel>
    {

        public DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            SetContentView(Resource.Layout.RecoverPasswordView);

            LinearLayout layo = this.FindViewById<LinearLayout>(Resource.Id.mainLayout);
            layo.Touch += Layo_Touch;

            var btn = FindViewById<Button>(Resource.Id.btnSendCode);
            btn.Click += Btn_Click;

            btn = FindViewById<Button>(Resource.Id.btnVerifyCode);
            btn.Click += Btn_Click;

            btn = FindViewById<Button>(Resource.Id.btnResend);
            btn.Click += Btn_Click;

            btn = FindViewById<Button>(Resource.Id.btnNewpassword);
            btn.Click += Btn_Click;

            btn = FindViewById<Button>(Resource.Id.btnLogin);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            HideSoftKeyboard();
        }

        private void Layo_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            HideSoftKeyboard();
        }

        public void HideSoftKeyboard()
        {
            try
            {
                if (CurrentFocus == null)
                    return;

                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

                CurrentFocus.ClearFocus();
            }
            catch (System.Exception)
            {

            }

        }
    }
}
