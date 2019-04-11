using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class CountryDto
    {
        public EnumCountries Country { get; private set; }
        public string CountryName { get; private set; }
        public string CountryFlagFile { get; private set; }
        public string CountryCode
        {
            get
            {
                string result = "+" + ((Int16)Country).ToString();
                return result;
            }
        }

        public CountryDto(EnumCountries country)
        {
            this.Country = country;
            this.CountryName = Helper.HelperClass.GetCountryName(country);
            this.CountryFlagFile = Helper.HelperClass.GetCountryFlagFile(country);
        }
    }
}
