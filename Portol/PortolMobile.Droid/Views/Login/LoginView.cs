using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.ViewModels;
using PortolMobile.Core.ViewModels.Login;

namespace PortolMobile.Droid.Views.Login
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("PortolMobile.Droid.Views.Login.LoginView")]
    public class LoginView : BaseFragment<LoginViewModel>
    {
        protected override int FragmentId => Resource.Layout.LoginView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.IsLoginPages = true;
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            //var _imageView = view.FindViewById<MvxCachedImageView>(Resource.Id.imgLogin);

            //ImageService.Instance
            //       .LoadCompiledResource("logo_long_white.png")
            //       .Into(_imageView);
            //  ParentActivity.SupportActionBar.Title = StringResc.Login;

            return view;
        }
      
    }
}
