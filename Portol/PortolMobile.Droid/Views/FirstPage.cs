
using Android.InputMethodServices;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.ViewModels;


namespace PortolMobile.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("PortolMobile.Droid.Views.FirstPage")]
    public class FirstPage : BaseFragment<FirstPageViewModel>
    {
        protected override int FragmentId => Resource.Layout.FirstPage;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            ParentActivity.SupportActionBar.Title = "asd";         

            return view;
        }      
                     
    }
}
