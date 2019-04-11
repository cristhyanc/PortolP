using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using PortolMobile.Forms.Droid.Effects;


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
            Control.Background = null;
        }

        protected override void OnDetached()
        {
            if (Control == null)
                return;

            Control.Background = oldBackground;
        }
    }
}