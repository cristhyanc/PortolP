using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class AddressFinderDto
    {
        public List<AddressFinderResultDto> completions { get; set; }
        public string paid { get; set; }
        public string success { get; set; }
    }

    public class AddressFinderResultDto
    {
        public string id { get; set; }
        public string full_address { get; set; }
        public string canonical_address_id { get; set; }

        string _AddressFirstPart="";
        string _AddressSecondPart = "";
        public string AddressFirstPart {
            get
            {
                DivideAddress();
                return _AddressFirstPart;
            }
        }

        public string AddressSecondPart
        {
            get
            {
                DivideAddress();
                return _AddressSecondPart;
            }
        }

        private void DivideAddress()
        {
            if (!string.IsNullOrEmpty(full_address) && string.IsNullOrEmpty(_AddressFirstPart))
            {
                var parts = full_address.Split(',');
                if (parts.Length > 1)
                {
                    _AddressFirstPart = full_address.Replace(parts[parts.Length - 1], "");
                    _AddressSecondPart = parts[parts.Length - 1];
                }
                else
                {
                    _AddressFirstPart = full_address;
                    _AddressSecondPart = "";
                }

            }
        }

    }

    public class AddressFinderDetail
    {
        public string locality_name { get; set; }
        public string state_territory { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string postcode { get; set; }


        public AddressDto GetAddressDto()
        {
            AddressDto result = new AddressDto();

            if (!string.IsNullOrEmpty(address_line_2))
            {
                result.FlatNumber = address_line_1;
                result.StreetName = address_line_2;
            }
            else
            {
                result.StreetName = address_line_1;
            }

            result.State = state_territory;
            result.Suburb = locality_name;
            result.PostCode = postcode;
            result.AddressValidated = true;
            result.Country = "AU";
            return result;
        }
    }
}
