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
                customer.CurrentAddress = _uow.AddressRepository.Get(x => x.ParentID == customer.CustomerID && x.ParentAddressType==ParentType.Customer );

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
                result.CustomerAddress.AddressID = user.CurrentAddress.AddressID;
                result.CustomerAddress.ParentID = user.CurrentAddress.ParentID;
                result.CustomerAddress.AddressValidated = user.CurrentAddress.AddressValidated;
                result.CustomerAddress.FullAddress = user.CurrentAddress.FullAddress;
                result.CustomerAddress.Latitude = user.CurrentAddress.Latitude;
                result.CustomerAddress.Longitude = user.CurrentAddress.Longitude;                
            }
           
            return result;
        }

        public static Customer Create(CustomerDto newUser, IUnitOfWork uow, byte[] passwordHash, byte[] passwordSalt)
        {
            if (newUser == null)
            {
                return null;
            }

            Customer user = new Customer();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
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
                user.CurrentAddress.ParentID = newUser.CustomerID;
                user.CurrentAddress.FullAddress = newUser.CustomerAddress.FullAddress;
                user.CurrentAddress.Latitude = newUser.CustomerAddress.Latitude;
                user.CurrentAddress.Longitude = newUser.CustomerAddress.Longitude;
                user.CurrentAddress.IsCurrentAddress = true;
                user.CurrentAddress.ParentAddressType = ParentType.Customer;
                user.CurrentAddress.AddressValidated = newUser.CustomerAddress.AddressValidated;
            }

            if (user.DOB == DateTime.MinValue)
            {
                throw new AppException(StringResources.DOBRequired);
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new AppException(StringResources.EmailRequired);
            }

            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new AppException(StringResources.FirstNameRequired);
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new AppException(StringResources.LastNameRequired);
            }

            if (user.PhoneNumber == 0)
            {
                throw new AppException(StringResources.MobileNumberRequiered);
            }


            if (uow.CustomerRepository.Get(x => x.PhoneNumber == user.PhoneNumber && x.PhoneCountryCode == user.PhoneCountryCode) != null)
            {
                throw new AppException(string.Format(StringResources.MobileInUse, user.PhoneNumber));
            }

            if (uow.CustomerRepository.Get(x => x.Email == user.Email) != null)
            {
                throw new AppException(string.Format(StringResources.EmailInUsedParameter, user.Email));
            }


            uow.CustomerRepository.Insert(user);

            if (user.CurrentAddress != null)
            {
                newUser.CustomerAddress.IsCurrentAddress = true;
                newUser.CustomerAddress.ParentID = user.CustomerID;
                user.CurrentAddress = Address.Create(newUser.CustomerAddress, ParentType.Customer, uow);               
            }
            return user;
        }

    }


}
