using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PortolMobile.Core.ViewModels.Login;
using System;

using UIKit;

namespace PortolMobile.iOS.Views.Login
{
 
    [MvxRootPresentation]
    public partial class LoginView : MvxViewController<LoginViewModel>
    {
        public LoginView() : base(nameof(LoginView), null)
        {
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                if (value)
                {
                    actIndicatior.StartAnimating();
                }
                else
                {
                    actIndicatior.StopAnimating();
                }

                actIndicatior.Hidden = !value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.AddGestureRecognizer(new UITapGestureRecognizer(()=>
            {
                txtEmail.ResignFirstResponder();
                txtPassword.ResignFirstResponder();
            }));

            txtEmail.ShouldReturn = KeyBoardTextReturn;

            txtPassword.ShouldReturn = KeyBoardTextReturn;


            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(txtEmail).To(vm => vm.EmailText);
            set.Bind(txtPassword).To(vm => vm.PasswordText);
            set.Bind(btnLogin).To(vm => vm.LoginButtonCommand);
            set.Bind(this).For("IsBusy").To(vm => vm.IsBusy);
            set.Apply();


            // Perform any additional setup after loading the view, typically from a nib.
        }

        private bool KeyBoardTextReturn(UITextField fieldText)
        {
            fieldText.ResignFirstResponder();
            return true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

