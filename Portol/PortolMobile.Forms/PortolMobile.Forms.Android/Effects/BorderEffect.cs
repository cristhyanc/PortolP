using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using PortolMobile.Forms.Droid.Effects;

[assembly: ResolutionGroupName("PortolMobileFormsEffects")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace PortolMobile.Forms.Droid.Effects
{
    
    class BorderEffect : PlatformEffect
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