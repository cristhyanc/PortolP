using Portol.Common.DTO;
using Portol.Common.Helper;
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
        public Guid ParentID { get; set; }
        public bool AddressValidated { get; set; }
        public string FullAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsCurrentAddress { get; set; }
        public ParentType ParentAddressType { get; set; }
        public bool IsStarterPoint { get; set; }
        
        public static Address Create(AddressDto addressDto, ParentType parentAddressType, IUnitOfWork uow)
        {
            Address address = new Address();
            address.AddressValidated = addressDto.AddressValidated;
            address.ParentID = addressDto.ParentID;
            address.FullAddress = addressDto.FullAddress;
            address.Latitude = addressDto.Latitude;
            address.Longitude = addressDto.Longitude;
            address.IsStarterPoint = addressDto.IsStarterPoint;
            address.IsCurrentAddress = addressDto.IsCurrentAddress;
            address.ParentAddressType = parentAddressType;
            uow.AddressRepository.Insert(address);
            return address;
        }

    }
}
