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
                view.HeightRequest = 40;
                var controlEntry = Control as UITextField;
                controlEntry.Layer.BorderWidth = 0;
                controlEntry.BorderStyle = UITextBorderStyle.None;
            }

            if (Element is DatePicker view2)
            {
                view2.HeightRequest = 40;
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