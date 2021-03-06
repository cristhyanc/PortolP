﻿using Portol.Common;
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
    public class CustomerService : ICustomerService, IDisposable
    {
        private IUnitOfWork _uow;       
        IImageManager _imageManager;
        public CustomerService(IUnitOfWork uow, IImageManager imageManager)
        {
            _uow = uow;
            _imageManager = imageManager;          
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _uow.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        public CustomerDto GetCustomerByEmail(string email)
        {
            var result = Customer.GetCustomerByEmail(_uow, email);
          
            if (result != null)
            {

                return result.ToDto();
            }

            return null;
        }

        public CustomerDto GetCustomer(Guid customerID)
        {
            var result = Customer.GetCustomer(_uow, customerID);

            if (result != null)
            {
                return result.ToDto();
            }

            return null;
        }

        public CustomerDto GetCustomerByPhoneNumber(long phoneNumber, int countryCode)
        {
            var result = Customer.GetCustomerByPhoneNumber(_uow, phoneNumber, countryCode);          
            if (result != null)
            {               
                return result.ToDto();
            }

            return null;
        }

        public CustomerDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);               
            }
                

            var user = Customer.GetCustomerByEmail(_uow, username);
            if (user == null)
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }               
                      
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }
                      
            return user.ToDto();
        }

        public IEnumerable<CustomerDto> GetAll()
        {
            var result = _uow.CustomerRepository.GetAll(x => !x.Deleted).Select(x => x.ToDto()).ToList();
            return result;
        }

        public CustomerDto GetById(Guid userId)
        {
            var user = _uow.CustomerRepository.Get(x => x.CustomerID == userId);
            return user.ToDto();

        }

        public bool SaveCustomer(CustomerDto user)
        {
            if (user.ProfilePhoto?.ImageArray?.Length > 0)
            {
                user.ProfilePhoto.ImageUrl = _imageManager.SaveFile(user.ProfilePhoto.ImageArray, user.CustomerID.ToString(), DateTime.Now.ToString("ddMMyyyyhhmmss") + ".jpg", ParentType.Customer);
            }

            if (new Customer().Save(user, _uow))
            { 
                _uow.SaveChanges();
                return true;
            }
            return false;

        }

        public bool SavePaymentMethod(PaymentMethodDto paymentMethod)
        {
            if (new PaymentMethod().Save(paymentMethod, _uow))
            {
                _uow.SaveChanges();
                return true;
            }
            return false;

        }

        public CustomerDto Create(CustomerDto newUser, string password)
        {

            if (string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.PasswordRequired);
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            Customer user = new Customer().Create(newUser, _uow, passwordHash, passwordSalt);

            _uow.SaveChanges();

            newUser = user.ToDto();
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