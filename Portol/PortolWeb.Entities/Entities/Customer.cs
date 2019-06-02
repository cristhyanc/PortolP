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
        public string CustomerPaymentID { get; set; }
        public string ProfilePhoto { get; set; }

        

        [NotMapped]
        public Address CurrentAddress { get; set; }
        //  [ForeignKey("AddressID")]

        [NotMapped]
        public ICollection<Address> CustomerAddresses { get; set; }

        public static Customer GetCustomerByPhoneNumber(IUnitOfWork _uow, long phoneNumber, int countryCode)
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
            if (customer != null)
            {
                customer = GetCustomerDetails(_uow, customer.CustomerID);
            }
            return customer;
        }

        public static Customer GetCustomer(IUnitOfWork _uow, Guid customerID)
        {
            var customer = _uow.CustomerRepository.Get(x => x.CustomerID== customerID);
            if (customer != null)
            {
                customer = GetCustomerDetails(_uow, customer.CustomerID);
            }
            return customer;
        }

        public static Customer GetCustomerDetails(IUnitOfWork _uow, Guid customerId)
        {
            var customer = _uow.CustomerRepository.Get(x => x.CustomerID == customerId);
            if (customer != null)
            {
                customer.CurrentAddress = _uow.AddressRepository.Get(x => x.ParentID == customer.CustomerID && x.ParentAddressType == ParentType.Customer);

            }
            return customer;
        }

        public  CustomerDto ToDto( )
        {
           

            CustomerDto result = new CustomerDto();
            result.DOB = this.DOB;
            result.Deleted = this.Deleted;
            result.Email = this.Email;
            result.FirstName = this.FirstName;
            result.LastName = this.LastName;
            result.ProfilePhoto = new PictureDto { ImageUrl = this.ProfilePhoto };
            result.PhoneCountryCode = this.PhoneCountryCode;
            result.PhoneNumber = this.PhoneNumber;
            result.CustomerID = this.CustomerID;
            result.CustomerPaymentID = this.CustomerPaymentID;
            result.IsGuess = this.IsGuess;
            if (this.CurrentAddress != null)
            {
                result.CustomerAddress.AddressID = this.CurrentAddress.AddressID;
                result.CustomerAddress.ParentID = this.CurrentAddress.ParentID;
                result.CustomerAddress.AddressValidated = this.CurrentAddress.AddressValidated;
                result.CustomerAddress.FullAddress = this.CurrentAddress.FullAddress;
                result.CustomerAddress.Latitude = this.CurrentAddress.Latitude;
                result.CustomerAddress.Longitude = this.CurrentAddress.Longitude;
            }

            return result;
        }

        public  bool  Save(CustomerDto newUser, IUnitOfWork uow)
        {
            if (newUser == null)
            {
                return false;
            }

            var dbuser = uow.CustomerRepository.Get(newUser.CustomerID);
            var user = PreValidations(newUser);

            if (uow.CustomerRepository.Get(x => x.CustomerID!= newUser.CustomerID && x.PhoneNumber == newUser.PhoneNumber && x.PhoneCountryCode == newUser.PhoneCountryCode) != null)
            {
                throw new AppException(string.Format(StringResources.MobileInUse, newUser.PhoneNumber));
            }

            if (uow.CustomerRepository.Get(x => x.CustomerID != newUser.CustomerID && x.Email == newUser.Email) != null)
            {
                throw new AppException(string.Format(StringResources.EmailInUsedParameter, newUser.Email));
            }


            user.PasswordHash = dbuser.PasswordHash;
            user.PasswordSalt = dbuser.PasswordSalt;
            uow.CustomerRepository.Update(user);
            return true;
        }

        public  Customer PreValidations(CustomerDto newUser)
        {

            Customer user = new Customer();

            user.CustomerID = newUser.CustomerID;
            user.DOB = newUser.DOB;
            user.Email = newUser.Email;
            user.Deleted = newUser.Deleted;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.ProfilePhoto = newUser.ProfilePhoto?.ImageUrl;
            user.PhoneCountryCode = newUser.PhoneCountryCode;
            user.CustomerPaymentID = newUser.CustomerPaymentID;
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

            return user;

        }

        public  Customer Create(CustomerDto newUser, IUnitOfWork uow, byte[] passwordHash, byte[] passwordSalt)
        {
            if (newUser == null)
            {
                return null;
            }

            var user = PreValidations(newUser);

            if (uow.CustomerRepository.Get(x => x.PhoneNumber == user.PhoneNumber && x.PhoneCountryCode == user.PhoneCountryCode) != null)
            {
                throw new AppException(string.Format(StringResources.MobileInUse, user.PhoneNumber));
            }

            if (uow.CustomerRepository.Get(x => x.Email == user.Email) != null)
            {
                throw new AppException(string.Format(StringResources.EmailInUsedParameter, user.Email));
            }

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
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
