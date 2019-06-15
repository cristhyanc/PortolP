using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

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
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }

        public static Address Save(AddressDto addressDto, ParentType parentAddressType, IUnitOfWork uow)
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
            address.Line1 = addressDto.Line1;
            address.Line2 = addressDto.Line2;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.Country = addressDto.Country;
            address.PostCode = addressDto.PostCode;            

            if (addressDto.AddressID == Guid.Empty)
            {
                var oldAddress = uow.AddressRepository.GetAll(x => x.ParentAddressType == parentAddressType && x.ParentID == address.ParentID);
                if(oldAddress?.Count()>0)
                {
                    uow.AddressRepository.Delete(oldAddress);
                }
                address.AddressID = Guid.NewGuid();
                uow.AddressRepository.Insert(address);
            }
            else
            {
                address.AddressID = addressDto.AddressID;
                uow.AddressRepository.Update(address);
            }

            
            return address;
        }

        public static Address GetAddress(Guid addressID, ParentType parentAddressType, IUnitOfWork uow)
        {
            return uow.AddressRepository.Get(x => x.AddressID == addressID && x.ParentAddressType == parentAddressType);
        }

        public static Address GetDropoffAddress(Guid parentID, ParentType parentAddressType, IUnitOfWork uow)
        {
            return uow.AddressRepository.Get(x => x.ParentID == parentID && x.ParentAddressType == parentAddressType && !x.IsStarterPoint );
        }

        public static Address GetPickUpAddress(Guid parentID, ParentType parentAddressType, IUnitOfWork uow)
        {
            return uow.AddressRepository.Get(x => x.ParentID == parentID && x.ParentAddressType == parentAddressType && x.IsStarterPoint);
        }

        public AddressDto ToDto()
        {
            AddressDto addressDto = new AddressDto();
            addressDto.AddressID = this.AddressID;
            addressDto.AddressValidated = this.AddressValidated;
            addressDto.FullAddress = this.FullAddress;
            addressDto.IsCurrentAddress = this.IsCurrentAddress;
            addressDto.IsStarterPoint = this.IsStarterPoint;
            addressDto.Latitude = this.Latitude;
            addressDto.Longitude = this.Longitude;
            addressDto.ParentID  = this.ParentID;
            addressDto.Line1 = this.Line1;
            addressDto.Line2 = this.Line2;
            addressDto.City = this.City;
            addressDto.State = this.State;
            addressDto.Country = this.Country;
            addressDto.PostCode = this.PostCode;
            return addressDto;
        }

    }
}
