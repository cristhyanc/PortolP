using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


using MvvmCross.Converters;
using Portol.Common.Helper;
using UIKit;

namespace PortolMobile.iOS
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
                        return UIImage.FromBundle("ic_australia");
                    case EnumCountries.NewZealand:
                        return UIImage.FromBundle("ic_newzealand");
                    case EnumCountries.UnitedKingdom:
                        return UIImage.FromBundle("ic_unitedKingdom");
                    default:
                        return UIImage.FromBundle("ic_australia");
                }
            }
            return UIImage.FromBundle("ic_australia");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}