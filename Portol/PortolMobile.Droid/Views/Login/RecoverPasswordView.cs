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
            //var bindingSet = this.CreateBindingSet<RecoverPasswordView, RecoverPasswordViewModel>();

            //var lny = view.FindViewById<LinearLayout>(Resource.Id.lnyPhoneNumber);
            //bindingSet.Bind(lny).For(v => v.Visibility).To(vm => vm.IsMobileSectionVisible).WithConversion<MvxVisibilityValueConverter>();

            //lny = view.FindViewById<LinearLayout>(Resource.Id.lnyCode);
            //bindingSet.Bind(lny).For(v => v.Visibility).To(vm => vm.IsMobileSectionVisible).WithConversion<MvxInvertedVisibilityValueConverter>();

            //bindingSet.Apply();

            return view;
        }

    }
}
