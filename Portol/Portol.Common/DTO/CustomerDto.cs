﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            CustomerAddress = new AddressDto();
        }
        public Guid CustomerID { get; set; }
        public string CustomerPaymentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DOB { get; set; }
        public long  PhoneNumber { get; set; }
        public int PhoneCountryCode { get; set; }
        public string Password { get; set; }


        PictureDto  _profilePhoto = new PictureDto {};
        public PictureDto ProfilePhoto
        {
            get { return _profilePhoto; }
            set { _profilePhoto = value; }
        }     

        public AddressDto CustomerAddress { get; set; }
        public bool Deleted { get; set; }
        public bool IsGuess { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
