using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PortolMobile.Core.ViewModels.Login;
using System;

using UIKit;

namespace PortolMobile.iOS.Views.Login
{
 
    public partial class LoginView : MvxViewController<LoginViewModel>
    {
        public LoginView() : base(nameof(LoginView), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

