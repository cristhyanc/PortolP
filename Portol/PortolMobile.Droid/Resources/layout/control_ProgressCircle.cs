using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PortolMobile.Droid.Resources.layout
{
    public class control_ProgressCircle : View
    {
        public control_ProgressCircle(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public control_ProgressCircle(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }
    }
}