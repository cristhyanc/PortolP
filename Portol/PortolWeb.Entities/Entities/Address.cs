using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblAddress")]
    public class Address
    {
        [Key]
        public Guid AddressID { get; set; }
        public string FlatNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }        
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public bool AddressValidated { get; set; }
        public bool IsCurrentAddress { get; set; }        

       // [ForeignKey("AddressID")]
        public Guid CustomerID { get; set; }

    }
}
