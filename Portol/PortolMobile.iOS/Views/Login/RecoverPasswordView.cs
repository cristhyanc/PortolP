using System;
using System.Collections.Generic;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PortolMobile.Core.ViewModels.Login;
using PortolMobile.iOS.Controls;
using UIKit;

namespace PortolMobile.iOS.Views.Login
{
    [MvxChildPresentation]
    public partial class RecoverPasswordView : MvxViewController<RecoverPasswordViewModel>
    {
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
                   // actIndicatior.StartAnimating();
                }
                else
                {
                  //  actIndicatior.StopAnimating();
                }

               // actIndicatior.Hidden = !value;
            }
        }

        public RecoverPasswordView() : base(nameof(RecoverPasswordView), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            List<string> list = new List<string>();
            list.Add("asd1");
            list.Add("asd2");
            list.Add("asd3");
            var examplePVM = new PIckeCountryModel(list);
           // ddps.Model = examplePVM;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

