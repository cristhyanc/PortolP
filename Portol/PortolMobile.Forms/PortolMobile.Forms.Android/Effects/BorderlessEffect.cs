using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: ExportEffect(typeof(BorderlessEffect), "BorderlessEffect")]
namespace PortolMobile.Forms.Droid.Effects
{
    class BorderlessEffect : PlatformEffect
    {
        Drawable oldBackground;
        protected override void OnAttached()
        {
            if (Control == null)
                return;

            oldBackground = Control.Background;
            Control.SetBackgroundResource(Droid.Resource.Drawable.EntryBottomBorder);
        }

        protected override void OnDetached()
        {
            if (Control == null)
                return;

            Control.Background = oldBackground;
        }
    }
}