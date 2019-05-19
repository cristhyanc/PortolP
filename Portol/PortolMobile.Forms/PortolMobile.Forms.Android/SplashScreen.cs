using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;


namespace PortolMobile.Forms.Droid
{
    [Activity(Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
              MainLauncher = true, //Set it as boot activity
              NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);          
            this.StartActivity(typeof(MainActivity));          
        }
    }
}