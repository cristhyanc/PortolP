using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Content.Res;
using Android.Views;
using Android.Widget;
using MvvmCross.Converters;
using Portol.Common.Helper;

namespace PortolMobile.Droid
{
    public class CountryFlagConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EnumCountries country)
            {
                switch (country)
                {
                    case EnumCountries.Australia:
                        return Application.Context.GetDrawable(Resource.Drawable.ic_australia);
                    case EnumCountries.NewZealand:
                        return Application.Context.GetDrawable(Resource.Drawable.ic_newZealand);
                    case EnumCountries.UnitedKingdom:
                        return Application.Context.GetDrawable(Resource.Drawable.ic_unitedKingdom); 
                    default:
                        return Application.Context.GetDrawable(Resource.Drawable.ic_australia);
                }                
            }
            return Application.Context.GetDrawable(Resource.Drawable.ic_australia);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}