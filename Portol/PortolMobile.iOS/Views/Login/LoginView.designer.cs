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
    [Register ("LoginView")]
    partial class LoginView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblStr { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblStr != null) {
                lblStr.Dispose ();
                lblStr = null;
            }
        }
    }
}