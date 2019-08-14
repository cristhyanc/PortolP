using System;
using PortolMobile.Forms.Controls;
using PortolMobile.Forms.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedContentPage), typeof(ExtendedPageRender))]
namespace PortolMobile.Forms.iOS.CustomRenderer
{
    public class ExtendedPageRender : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController != null)
                NavigationController.TopViewController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null);
        }

    }
}