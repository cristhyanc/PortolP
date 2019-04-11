using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        [ForeignKey("BusinessID")]
        public Guid BusinessID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public long PhoneNumber { get; set; }
        public int PhoneCountryCode { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FlatNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool Deleted { get; set; }
        public bool AddressValidated { get; set; }

        
        public static UserDto ORM(User user )
        {
            if(user==null)
            {
                return null;
            }

            UserDto result = new UserDto();            
            result.DOB = user.DOB;
            result.Deleted = user.Deleted;
            result.Email = user.Email;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.PhoneCountryCode = user.PhoneCountryCode;
            result.PhoneNumber = user.PhoneNumber;
            result.UserID = user.UserID;          
            result.UserAddress.PostCode = user.PostCode;
            result.UserAddress.Country = user.Country;
            result.UserAddress.FlatNumber = user.FlatNumber;
            result.UserAddress.State = user.State;
            result.UserAddress.StreetName = user.StreetName;
            result.UserAddress.Suburb = user.Suburb;
            result.UserAddress.AddressValidated = user.AddressValidated;
            return result;
        }

        public static User ORM(UserDto newUser)
        {
            if (newUser == null)
            {
                return null;
            }
            User user = new User();          
            user.DOB = newUser.DOB;
            user.Email = newUser.Email;
            user.Deleted = newUser.Deleted;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.PhoneCountryCode = newUser.PhoneCountryCode;
            user.PhoneNumber = newUser.PhoneNumber;

            if (newUser.UserAddress != null)
            {
                user.PostCode = newUser.UserAddress.PostCode;
                user.Country = newUser.UserAddress.Country;
                user.FlatNumber = newUser.UserAddress.FlatNumber;
                user.State = newUser.UserAddress.State;
                user.StreetName = newUser.UserAddress.StreetName;
                user.Suburb = newUser.UserAddress.Suburb;
                user.AddressValidated = newUser.UserAddress.AddressValidated;
            }
            return user;
        }

    }


}
