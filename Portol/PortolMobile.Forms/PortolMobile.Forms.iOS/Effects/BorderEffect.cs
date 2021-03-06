﻿using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using CoreGraphics;
using PortolMobile.Forms.iOS.Effects;

[assembly: ResolutionGroupName("PortolMobileFormsEffects")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace PortolMobile.Forms.iOS.Effects
{
   
    class BorderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {


        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (Element is Entry view)
            {
                if ((args.PropertyName.Equals("Height") || args.PropertyName.Equals("Width"))
                && !isBorderSetted && view.Width > 1 && view.Height > 1)
                {
                    SetBorder();
                }
            }



        }
        private bool isBorderSetted = false;
        private void SetBorder()
        {

            if (Element is Entry view)
            {
                isBorderSetted = true;
                view.HeightRequest = 35;
                var controlEntry = Control as UITextField;
                var borderSize = 1;

                CALayer border = new CALayer
                {
                    BorderColor = Helper.GetUIColor("#ffffff").CGColor,
                    Frame = new CGRect(x: 0, y: view.Height + 2, width: view.Width, height: 1.0f),
                    BorderWidth = borderSize
                };

                controlEntry.Layer.AddSublayer(border);

                controlEntry.Layer.MasksToBounds = true;
                controlEntry.BorderStyle = UITextBorderStyle.None;

            }


        }

        protected override void OnDetached()
        {

        }
    }
}