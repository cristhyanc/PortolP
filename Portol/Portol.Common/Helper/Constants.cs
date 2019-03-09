using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace Portol.Common.Helper
{
    public class Constants
    {
       

        public const string BaseUrl = "http://192.168.1.120/PortolWeb/api/";
        // public const string BaseUrl = "http://desktop-mkvm17a/PortolWeb/api/";
        public const string BaseUserApiUrl = BaseUrl + "users";

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
                    return "asd";
                case EnumCountries.NewZealand:
                    return "123";
                case EnumCountries.UnitedKingdom:
                    return "345";
                default:
                    return "";
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
