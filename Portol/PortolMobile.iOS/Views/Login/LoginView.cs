using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PortolMobile.Core.ViewModels.Login;
using System;

using UIKit;

namespace PortolMobile.iOS.Views.Login
{
 
    [MvxRootPresentation(WrapInNavigationController =true)]
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

            this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("Login_background"));

            this.View.AddGestureRecognizer(new UITapGestureRecognizer(()=>
            {
                txtEmail.ResignFirstResponder();
                txtPassword.ResignFirstResponder();
               
            }));



            txtEmail.ShouldReturn = KeyBoardTextReturn;
            txtPassword.ShouldReturn = KeyBoardTextReturn;

            btnLogin.TouchUpInside += Button_Touched;
            btnSignup.TouchUpInside += Button_Touched;
            btnForgotPassword.TouchUpInside += Button_Touched;


            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(txtEmail).To(vm => vm.EmailText);
            set.Bind(txtPassword).To(vm => vm.PasswordText);
            set.Bind(btnLogin).To(vm => vm.LoginButtonCommand);
            set.Bind(btnForgotPassword).To(vm => vm.RecoverButtonCommand);
            set.Bind(this).For("IsBusy").To(vm => vm.IsBusy);
            set.Apply();

            txtEmail.AttributedPlaceholder = new NSAttributedString("Email", null, UIColor.LightGray);
            txtPassword.AttributedPlaceholder = new NSAttributedString("Password", null, UIColor.LightGray);
            txtEmail.Layer.BorderColor = UIColor.White.CGColor;
            txtEmail.Layer.BorderWidth = 1f;

            txtPassword.Layer.BorderColor = UIColor.White.CGColor;
            txtPassword.Layer.BorderWidth = 1f;
          
        }

        private void Button_Touched(object sender, EventArgs e)
        {
            View.EndEditing(true);
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

