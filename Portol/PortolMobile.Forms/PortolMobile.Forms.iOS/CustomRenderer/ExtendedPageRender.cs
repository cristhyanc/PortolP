using System;
using PortolMobile.Forms.Controls;
using PortolMobile.Forms.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
/*
[assembly: ExportRenderer(typeof(ExtendedContentPage), typeof(ExtendedPageRender))]
namespace PortolMobile.Forms.iOS.CustomRenderer
{
    public class ExtendedPageRender : PageRenderer
    {
        private string backgroundurl = "";
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }


            var page = e.NewElement as ExtendedContentPage;
            backgroundurl = page.BackgroundImage;



        }

        

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(false);

            if (!string.IsNullOrEmpty(backgroundurl))
            {
                UIGraphics.BeginImageContext(View.Bounds.Size);
                UIImage i = UIImage.FromFile(backgroundurl);

                if (i != null && View != null)
                {
                    i = i.Scale(View.Bounds.Size);
                    if (i != null)
                    {
                        View.BackgroundColor = UIColor.FromPatternImage(i);
                    }

                }
            }

        }



    }
}*/