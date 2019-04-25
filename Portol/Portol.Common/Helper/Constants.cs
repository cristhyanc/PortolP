using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace Portol.Common.Helper
{
    public class Constants
    {
        public const string RegexEmailPattern = @"@";

        public const string PortolDomain = "https://portolwebapitest.azurewebsites.net";
        public const string BaseAddressApiUrl = "https://api.addressfinder.io/api/au/address";

       //   public const string BaseUrl = "https://portolwebapitest.azurewebsites.net";
        public const string BaseUrl = "http://192.168.8.100/PortolWeb";
        // public const string BaseUrl = "http://desktop-mkvm17a/PortolWeb";
        public const string BaseUserApiUrl = BaseUrl + "/api/users";

        static ReadOnlyCollection<CountryDto> _countryList;
        public static ReadOnlyCollection<CountryDto> CountryList
        {
            get

            {
                if (_countryList == null)
                {
                    var countries = new List<CountryDto>();

                    var country = new CountryDto(EnumCountries.Australia );
                    countries.Add(country);

                    country = new CountryDto(EnumCountries.NewZealand);
                    countries.Add(country);

                    country = new CountryDto(EnumCountries.UnitedKingdom );
                    countries.Add(country);
                    _countryList = countries.AsReadOnly();
                }

                return _countryList;
            }
        }


    }

    public class HelperClass
    {
        public static string GetCountryName(EnumCountries enumCountries)
        {
            switch (enumCountries)
            {
                case EnumCountries.Australia:
                    return StringResources.Australia;                   
                case EnumCountries.NewZealand:
                    return StringResources.NewZealand;
                case EnumCountries.UnitedKingdom:
                    return StringResources.UnitedKingdom;
                default:
                    return "";
            }
        }

        public static string GetCountryFlagFile(EnumCountries enumCountries)
        {
            switch (enumCountries)
            {
                case EnumCountries.Australia:
                    return "resource://PortolMobile.Forms.Resources.ic_australia.svg?assembly=PortolMobile.Forms";
                case EnumCountries.NewZealand:
                    return "resource://PortolMobile.Forms.Resources.ic_newzealand.svg?assembly=PortolMobile.Forms";
                case EnumCountries.UnitedKingdom:
                    return "resource://PortolMobile.Forms.Resources.ic_unitedkingdom.svg?assembly=PortolMobile.Forms";
                default:
                    return "resource://PortolMobile.Forms.Resources.ic_australia.svg?assembly=PortolMobile.Forms";
            }
        }
    }

    public enum EnumCountries
    {
        Australia = 61,
        NewZealand = 64,
        UnitedKingdom = 44,
    }

}
