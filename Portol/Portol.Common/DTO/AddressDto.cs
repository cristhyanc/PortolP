using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class AddressDto
    {
        public string FlatNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public bool  AddressValidated { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }

        public string FullAddress
        {
            get {
                string result = "";

                if(!string.IsNullOrEmpty(FlatNumber ))
                {
                    result = FlatNumber;
                }

                if (!string.IsNullOrEmpty(StreetName))
                {
                    result +=" " + StreetName;
                }

                if (!string.IsNullOrEmpty(Suburb))
                {
                    result += " " + Suburb;
                }

                //if (!string.IsNullOrEmpty(City))
                //{
                //    result += " " + City;
                //}

                if (!string.IsNullOrEmpty(State))
                {
                    result += " " + State;
                }

                if (!string.IsNullOrEmpty(PostCode))
                {
                    result += " " + PostCode;
                }

                return result;
            }
        }
    }
}
