﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.DTO
{
    public class UserDto
    {

        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public int PhoneNumber { get; set; }
        public int PhoneCountryCode { get; set; }
        public string Password { get; set; }
        public AddressDto UserAddress { get; set; }
    }
}
