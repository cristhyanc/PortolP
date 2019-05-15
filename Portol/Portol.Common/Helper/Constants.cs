using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace Portol.Common.Helper
{
    public class MessagingCenterCodes
    {
        public const string AddressPickerMessage = "AddressPickerMessage";
        public const string PicturePickerMessage = "PicturePickerMessage";
        public const string MeasurementMessage = "PicturePickerMessage";
    }

    public class Constants
    {
        public const string RegexEmailPattern = @"@";

        public const string PortolDomain = "https://portolwebapitest.azurewebsites.net";
        public const string BaseAddressApiUrl = "https://api.addressfinder.io/api/au/address";

       //   public const string BaseUrl = "https://portolwebapitest.azurewebsites.net";
       public const string BaseUrl = "http://192.168.8.100/PortolWeb";
      //    public const string BaseUrl = "http://192.168.43.31/portolweb";
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


        public static string GetCreditCardResourceFile(PaymentMethodType creditCardType)
        {
            switch (creditCardType)
            {
                case PaymentMethodType.MasterCard:
                    return "resource://PortolMobile.Forms.Resources.ic_mastercard.svg?assembly=PortolMobile.Forms";
                case PaymentMethodType.Visa:
                    return "resource://PortolMobile.Forms.Resources.ic_visa.svg?assembly=PortolMobile.Forms";
                case PaymentMethodType.PayPal:
                    return "resource://PortolMobile.Forms.Resources.ic_paypal.svg?assembly=PortolMobile.Forms";
                default:
                    return "resource://PortolMobile.Forms.Resources.ic_creditcard.svg?assembly=PortolMobile.Forms";
            }
        }

        public static string GetCreditCardRFileName(PaymentMethodType creditCardType)
        {
            switch (creditCardType)
            {
                case PaymentMethodType.MasterCard:
                    return "ic_mastercard.png";
                case PaymentMethodType.Visa:
                    return "ic_visa.png";
                case PaymentMethodType.PayPal:
                    return "ic_paypal.png";
                default:
                    return "ic_creditcard.png";
            }
        }
    }

    public enum PaymentMethodType
    {
        MasterCard = 1,
        Visa = 2,
        PayPal = 3,
    }

    public enum EnumCountries
    {
        Australia = 61,
        NewZealand = 64,
        UnitedKingdom = 44,
    }

}
