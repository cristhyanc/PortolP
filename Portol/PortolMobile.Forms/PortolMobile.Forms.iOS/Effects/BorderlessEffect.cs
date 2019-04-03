using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using CoreGraphics;
using PortolMobile.Forms.iOS.Effects;


[assembly: ExportEffect(typeof(BorderlessEffect), "BorderlessEffect")]
namespace PortolMobile.Forms.iOS.Effects
{

    class BorderlessEffect : PlatformEffect
    {
        protected override void OnAttached()
        {


        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (Element is Entry view)
            {
              SetBorder();
            }
        }
       
        private void SetBorder()
        {

            if (Element is Entry view)
            { 
                var controlEntry = Control as UITextField;
                controlEntry.Layer.BorderWidth = 0;
                controlEntry.BorderStyle = UITextBorderStyle.None;
            }


        }

        protected override void OnDetached()
        {

        }
    }
}