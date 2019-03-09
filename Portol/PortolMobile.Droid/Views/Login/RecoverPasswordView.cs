using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Plugin.Visibility;
using PortolMobile.Core.ViewModels;
using PortolMobile.Core.ViewModels.Login;

namespace PortolMobile.Droid.Views.Login
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("PortolMobile.Droid.Views.Login.RecoverPasswordView")]
    public class RecoverPasswordView : BaseFragment<RecoverPasswordViewModel>
    {
        protected override int FragmentId => Resource.Layout.RecoverPasswordView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.IsLoginPages = true;
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var btn = view.FindViewById<Button>(Resource.Id.btnSendCode);
            btn.Click += Btn_Click;

            btn = view.FindViewById<Button>(Resource.Id.btnVerifyCode);
            btn.Click += Btn_Click;

            btn = view.FindViewById<Button>(Resource.Id.btnResend);
            btn.Click += Btn_Click;

            btn = view.FindViewById<Button>(Resource.Id.btnNewpassword);
            btn.Click += Btn_Click;

            btn = view.FindViewById<Button>(Resource.Id.btnLogin);
            btn.Click += Btn_Click;
            
            return view;
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            ((MainView)Activity)?.HideSoftKeyboard();
        }
    }
}
