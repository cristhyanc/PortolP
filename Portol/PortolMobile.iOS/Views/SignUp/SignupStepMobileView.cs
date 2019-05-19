using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PortolMobile.Core.ViewModels.SignUp;
using UIKit;


namespace PortolMobile.iOS.Views.SignUp
{
    [MvxChildPresentation]
    public partial class SignupStepMobileView : MvxViewController<SignupStepMobileViewModel>
    {
        public SignupStepMobileView() : base(nameof(SignupStepMobileView), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            txtNumber.ShouldReturn = KeyBoardTextReturn;
            btnCountry.TouchUpInside += Button_Touched;
            var set = this.CreateBindingSet<SignupStepMobileView, SignupStepMobileViewModel>();
            set.Bind(btnCountry).To(vm => vm.SelectCountryCommand);
            set.Bind(btnCountry).For("Title").To(vm => vm.CountrySelected.CountryCode);
            set.Bind(ImgCountryFlag).To(vm => vm.CountrySelected.Country).WithConversion("CountryFlagConverter");
            set.Apply();
        }

        private bool KeyBoardTextReturn(UITextField fieldText)
        {
            fieldText.ResignFirstResponder();
            return true;
        }

        private void Button_Touched(object sender, EventArgs e)
        {
            View.EndEditing(true);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

