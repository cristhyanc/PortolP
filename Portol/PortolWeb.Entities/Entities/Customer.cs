using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblCustomer")]
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }
      //  [ForeignKey("BusinessID")]
        public Guid? BusinessID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public long PhoneNumber { get; set; }
        public int PhoneCountryCode { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }       
        public bool Deleted { get; set; }
        public bool IsGuess { get; set; }

        [NotMapped]
        public Address CurrentAddress { get; set; }
      //  [ForeignKey("AddressID")]
     
        [NotMapped]
        public ICollection<Address> CustomerAddresses { get; set; }


        public void CreateCustomer(IUnitOfWork _uow)
        {
            if (this.DOB == DateTime.MinValue)
            {
                throw new AppException(StringResources.DOBRequired);
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                throw new AppException(StringResources.EmailRequired);
            }

            if (string.IsNullOrEmpty(this.FirstName))
            {
                throw new AppException(StringResources.FirstNameRequired);
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                throw new AppException(StringResources.LastNameRequired);
            }

            if (this.PhoneNumber == 0)
            {
                throw new AppException(StringResources.MobileNumberRequiered);
            }
                      

            if (_uow.CustomerRepository.Get(x => x.PhoneNumber == this.PhoneNumber && x.PhoneCountryCode == this.PhoneCountryCode) != null)
            {
                throw new AppException(string.Format(StringResources.MobileInUse, this.PhoneNumber));
            }

            if (_uow.CustomerRepository.Get(x => x.Email == this.Email) != null)
            {
                throw new AppException(string.Format(StringResources.EmailInUsedParameter, this.Email));
            }
                     

            _uow.CustomerRepository.Insert(this);

            if (this.CurrentAddress != null)
            {
                this.CurrentAddress.IsCurrentAddress = true;
                this.CurrentAddress.CustomerID = this.CustomerID;
                _uow.AddressRepository.Insert(this.CurrentAddress);
            }
        }

        public static  Customer GetCustomerByPhoneNumber(IUnitOfWork _uow, long phoneNumber, int countryCode)
        {
            var customer = _uow.CustomerRepository.Get(x => x.PhoneNumber == phoneNumber && x.PhoneCountryCode == countryCode);
            if (customer != null)
            {
                customer = GetCustomerDetails(_uow, customer.CustomerID);
            }
            return customer;
        }

        public static Customer GetCustomerByEmail(IUnitOfWork _uow, string email)
        {
            var customer = _uow.CustomerRepository.Get(x => x.Email.Equals(email));
            if(customer!=null)
            {
                customer = GetCustomerDetails(_uow,customer.CustomerID);
            }            
            return customer;
        }

        public static Customer GetCustomerDetails(IUnitOfWork _uow, Guid customerId)
        {
            var customer = _uow.CustomerRepository.Get(x => x.CustomerID== customerId);
            if (customer != null)
            {
                customer.CurrentAddress = _uow.AddressRepository.Get(x => x.CustomerID == customer.CustomerID);

            }
            return customer;
        }

        public static CustomerDto ORM(Customer user )
        {
            if(user==null)
            {
                return null;
            }

            CustomerDto result = new CustomerDto();            
            result.DOB = user.DOB;
            result.Deleted = user.Deleted;
            result.Email = user.Email;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.PhoneCountryCode = user.PhoneCountryCode;
            result.PhoneNumber = user.PhoneNumber;
            result.CustomerID = user.CustomerID;
            result.IsGuess = user.IsGuess;
            if (user.CurrentAddress !=null)
            {
                result.CustomerAddress.PostCode = user.CurrentAddress.PostCode;
                result.CustomerAddress.Country = user.CurrentAddress.Country;
                result.CustomerAddress.FlatNumber = user.CurrentAddress.FlatNumber;
                result.CustomerAddress.State = user.CurrentAddress.State;
                result.CustomerAddress.StreetName = user.CurrentAddress.StreetName;
                result.CustomerAddress.Suburb = user.CurrentAddress.Suburb;
                result.CustomerAddress.AddressValidated = user.CurrentAddress.AddressValidated;
            }
           
            return result;
        }

        public static Customer ORM(CustomerDto newUser)
        {
            if (newUser == null)
            {
                return null;
            }
            Customer user = new Customer();          
            user.DOB = newUser.DOB;
            user.Email = newUser.Email;
            user.Deleted = newUser.Deleted;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.PhoneCountryCode = newUser.PhoneCountryCode;
            user.PhoneNumber = newUser.PhoneNumber;
            user.IsGuess = user.IsGuess;
            if (newUser.CustomerAddress != null)
            {
                user.CurrentAddress = new Address();
                user.CurrentAddress.CustomerID = newUser.CustomerID;
                user.CurrentAddress.PostCode = newUser.CustomerAddress.PostCode;
                user.CurrentAddress.Country = newUser.CustomerAddress.Country;
                user.CurrentAddress.FlatNumber = newUser.CustomerAddress.FlatNumber;
                user.CurrentAddress.State = newUser.CustomerAddress.State;
                user.CurrentAddress.StreetName = newUser.CustomerAddress.StreetName;
                user.CurrentAddress.Suburb = newUser.CustomerAddress.Suburb;
                user.CurrentAddress.AddressValidated = newUser.CustomerAddress.AddressValidated;
            }
            return user;
        }

    }


}
