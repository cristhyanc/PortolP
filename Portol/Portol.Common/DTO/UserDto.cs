using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.DTO
{
    public class UserDto
    {
        public UserDto()
        {
            UserAddress = new AddressDto();
        }
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DOB { get; set; }
        public long  PhoneNumber { get; set; }
        public int PhoneCountryCode { get; set; }
        public string Password { get; set; }
        public AddressDto UserAddress { get; set; }
        public bool Deleted { get; set; }
    }
}
