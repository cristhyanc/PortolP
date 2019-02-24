using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PortolMobile.Droid.Controls
{
  public  class ProgressCircle : RelativeLayout
    {
        //private ProgressBar  _progressBar;
        View _layout;

        public ProgressCircle(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public Boolean  IsVisible
        {
            get
            {
                if(this.Visibility==ViewStates.Visible )
                {
                    return true;
                }
                return false;
            }
            set
            {
                if(value)
                {
                    this.Visibility = ViewStates.Visible;
                }
                else
                {
                    this.Visibility = ViewStates.Invisible;
                }

            }
        }
             

        private void Init(IAttributeSet attrs)
        {
            var inflater = LayoutInflater.From(Context);
            _layout = inflater.Inflate(Resource.Layout.control_ProgressCircle, this);
           // _progressBar = layout.FindViewById<ProgressBar>(Resource.Id.progressCricle1);
          
        }
    }
}
