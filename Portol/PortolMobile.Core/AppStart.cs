using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PortolMobile.Core.ViewModels;
using PortolMobile.Core.ViewModels.Login;
using PortolMobile.Core.ViewModels.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core
{
   public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return NavigationService.Navigate<SignupStepMobileViewModel>();
        }
    }
}
