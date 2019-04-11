using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace PortolMobile.iOS.Controls
{

    [Register("UIBusyIndicator"), DesignTimeVisible(true)]
    public class UIBusyIndicator: UIActivityIndicatorView
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
                    StartAnimating();
                }
                else
                {
                    StopAnimating();
                }

                Hidden = !value;
            }
        }


    }
}
