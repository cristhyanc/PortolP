using Portol.Common;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using Portol.Common.DTO;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortolWeb.Core.UserServices
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork _uow;

        public CustomerService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool ValidateVerificationCode(long phoneNumber, Int32 countryCode, Int32 code)
        {
            var savedCode = _uow.CodeVerificationRepository.Get(x => x.CodeNumber == code && x.PhoneNumber == phoneNumber &&
                                                                        x.CountryCode == countryCode);
            if(savedCode==null)
            {
                return false;
            }

            _uow.CodeVerificationRepository.Delete(savedCode);
            _uow.SaveChanges();
            return true;
            
        }

        public CustomerDto GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {
            var customer = new Customer();
            var result = customer.GetCustomerByPhoneNumber(_uow, phoneNumber, countryCode);
            if (result != null)
            {
                return Customer.ORM(result);
            }

            return null;
        }

        public CustomerDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);               
            }
                

            var user = _uow.CustomerRepository.Get(x => x.Email == username);                      
            if (user == null)
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }               
                      
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }

            user.CurrentAddress = _uow.AddressRepository.Get(x => x.CustomerID.Equals(user.CustomerID) && x.IsCurrentAddress);
            return Customer.ORM(user);
         
        }

        public IEnumerable<CustomerDto> GetAll()
        {
            var result = _uow.CustomerRepository.GetAll(x => !x.Deleted).Select(x => Customer.ORM(x)).ToList();
            return result;
        }

        public CustomerDto GetById(Guid userId)
        {
            var user = _uow.CustomerRepository.Get(x => x.CustomerID == userId);
            return Customer.ORM(user);

        }

        public CustomerDto Create(CustomerDto newUser, string password)
        {

            if (string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.PasswordRequired);
            }

            Customer user = Customer.ORM(newUser);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
                                   
            user.CreateCustomer(_uow);
           
            _uow.SaveChanges();

            newUser.CustomerID = user.CustomerID;
            return newUser;
           
        }
        public bool VerifyMobileUniqueness(CustomerDto phoneDetails)
        {
            if (_uow.CustomerRepository.Get(x => x.PhoneNumber == phoneDetails.PhoneNumber && x.PhoneCountryCode== phoneDetails.PhoneCountryCode) != null)
            {
                return false;
            }
            return true;
        }

        public bool VerifyEmailUniqueness(string email)
        {
            if (_uow.CustomerRepository.Get(x => x.Email.Equals(email.Trim())) != null)
            {
                return false;
            }
            return true;
        }

       
        public void ResetPassword(CustomerDto user)
        {
            if(user==null)
            {
                throw new AppException(StringResources.PasswordRequired);
            }

            var currentUser = _uow.CustomerRepository.Get(x => x.PhoneNumber == user.PhoneNumber);
            if(currentUser==null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            if(string.IsNullOrEmpty(user.Password) )
            {
                throw new AppException(StringResources.PasswordRequired );
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password , out passwordHash, out passwordSalt);
            currentUser.PasswordHash = passwordHash;
            currentUser.PasswordSalt = passwordSalt;
            _uow.CustomerRepository.Update(currentUser);
            _uow.SaveChanges();
        }
        public void Update(CustomerDto userParam, string password = null)
        {
           
        }

        public void Delete(Guid userId)
        {
            var user = _uow.CustomerRepository.Get(x => x.CustomerID == userId);
            if(user==null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            user.Deleted = true;
            _uow.CustomerRepository.Update(user);
            _uow.SaveChanges();
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new AppException(StringResources.PasswordRequired );
            if (string.IsNullOrWhiteSpace(password)) throw new AppException(StringResources.PasswordWhiteSpace, StringResources.Password);

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new AppException(StringResources.PasswordRequired);
            if (string.IsNullOrWhiteSpace(password)) throw new AppException(StringResources.PasswordWhiteSpace, StringResources.Password);
            if (storedHash.Length != 64) throw new AppException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new AppException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}