using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.Resources;
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

            //ParentActivity.SupportActionBar.Title = Strings.TargetPlanets;


            return view;
        }
    }
}
