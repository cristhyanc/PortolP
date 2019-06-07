using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using PortolMobile.Forms.Controls;
using PortolMobile.Forms.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationRenderer))]
namespace PortolMobile.Forms.iOS.CustomRenderer
{
    public class CustomNavigationRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

          //  UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
          //  UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if(e.NewElement is CustomNavigationPage customNavigation)
            {
                if (customNavigation.IsBackgroundTransparent)
                {
                    NavigationBar.SetBackgroundImage(new UIKit.UIImage(), UIKit.UIBarMetrics.Default);
                    NavigationBar.ShadowImage = new UIKit.UIImage();
                    UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
                    UINavigationBar.Appearance.TintColor = UIColor.White;
                    UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
                    UINavigationBar.Appearance.Translucent = true;
                }
                else
                {
                    UINavigationBar.Appearance.BackgroundColor = UIColor.White;
                    UINavigationBar.Appearance.TintColor = UIColor.White;
                    UINavigationBar.Appearance.BarTintColor = UIColor.White;
                    UINavigationBar.Appearance.Translucent = false;
                }
                    
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}