using System;
using System.Collections.Generic;
using System.Text;


namespace Portol.Common.DTO
{
    public class AddressDto
    {
        //public string StreetName { get; set; }
        //public string Suburb { get; set; }
        //public string State { get; set; }
        //public string FlatNumber { get; set; }
       // public string Country { get; set; }
        //public string PostCode { get; set; }

        public bool  AddressValidated { get; set; }
        public string FullAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid AddressID { get; set; }
        public Guid ParentID { get; set; }
        public bool IsStarterPoint { get; set; }
        public bool IsCurrentAddress { get; set; }


    }
}
