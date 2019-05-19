// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PortolMobile.iOS.Views.Login
{
    [Register ("RecoverPasswordsView")]
    partial class RecoverPasswordsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSendCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImgCountryFlag { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtNumber { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCountry != null) {
                btnCountry.Dispose ();
                btnCountry = null;
            }

            if (btnSendCode != null) {
                btnSendCode.Dispose ();
                btnSendCode = null;
            }

            if (ImgCountryFlag != null) {
                ImgCountryFlag.Dispose ();
                ImgCountryFlag = null;
            }

            if (txtNumber != null) {
                txtNumber.Dispose ();
                txtNumber = null;
            }
        }
    }
}