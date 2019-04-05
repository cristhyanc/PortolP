using Portol.Common;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using Portol.DTO;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortolWeb.Core.UserServices
{
    public class UserService : IUserService
    {
        private IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool ValidateVerificationCode(long phoneNumber, Int32 countryCode, Int32 code)
        {
            var savedCode = _uow.CodeVerificationRepository.Get(x => x.CodeNumber == code && x.PhoneNumber == phoneNumber.ToString() &&
                                                                        x.CountryCode == countryCode.ToString());
            if(savedCode==null)
            {
                return false;
            }

            _uow.CodeVerificationRepository.Delete(savedCode);
            _uow.SaveChanges();
            return true;
            
        }

        public UserDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);               
            }
                

            var user = _uow.UserRepository.Get(x => x.Email == username);                      
            if (user == null)
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }               
                      
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException(StringResources.EmailPasswordIsIncorrect);
            }


            return User.ORM(user);
            // return user;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var result = _uow.UserRepository.GetAll(x => !x.Deleted).Select(x => User.ORM(x)).ToList();
            return result;
        }

        public UserDto GetById(Guid userId)
        {
            var user = _uow.UserRepository.Get(x => x.UserID == userId);
            return User.ORM(user);

        }

        public UserDto Create(UserDto newUser, string password)
        {
            if(newUser.DOB ==DateTime.MinValue )
            {
                throw new AppException(StringResources.DOBRequired);
            }

            if(string.IsNullOrEmpty(newUser.Email ))
            {
                throw new AppException(StringResources.EmailRequired);
            }

            if (string.IsNullOrEmpty(newUser.FirstName))
            {
                throw new AppException(StringResources.FirstNameRequired);
            }

            if (string.IsNullOrEmpty(newUser.LastName))
            {
                throw new AppException(StringResources.LastNameRequired);
            }

            if (newUser.PhoneNumber==0)
            {
                throw new AppException(StringResources.MobileNumberRequiered);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new AppException(StringResources.PasswordRequired);
            }

            if (_uow.UserRepository.Get(x => x.PhoneNumber  == newUser.PhoneNumber && x.PhoneCountryCode == newUser.PhoneCountryCode) != null)
            {
                throw new AppException(string.Format(StringResources.MobileInUse , newUser.PhoneNumber));
            }

            if (_uow.UserRepository.Get(x => x.Email == newUser.Email) != null)
            {
                throw new AppException(string.Format(StringResources.EmailInUsedParameter, newUser.Email));
            }


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            User user = User.ORM(newUser);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _uow.UserRepository.Insert(user);
            _uow.SaveChanges();

            newUser.UserID = user.UserID;
            return newUser;
           
        }
        public bool VerifyMobileUniqueness(UserDto phoneDetails)
        {
            if (_uow.UserRepository.Get(x => x.PhoneNumber == phoneDetails.PhoneNumber && x.PhoneCountryCode== phoneDetails.PhoneCountryCode) != null)
            {
                return false;
            }
            return true;
        }

        public bool VerifyEmailUniqueness(string email)
        {
            if (_uow.UserRepository.Get(x => x.Email.Equals(email.Trim())) != null)
            {
                return false;
            }
            return true;
        }

       
        public void ResetPassword(UserDto user)
        {
            if(user==null)
            {
                throw new AppException(StringResources.PasswordRequired);
            }

            var currentUser = _uow.UserRepository.Get(x => x.PhoneNumber == user.PhoneNumber);
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
            _uow.UserRepository.Update(currentUser);
            _uow.SaveChanges();
        }
        public void Update(UserDto userParam, string password = null)
        {
            // var user = _context.Users.Find(userParam.Id);

            //if (user == null)
            //    throw new AppException("User not found");

            //if (userParam.Username != user.Username)
            //{
            //    // username has changed so check if the new username is already taken
            //    if (_context.Users.Any(x => x.Username == userParam.Username))
            //        throw new AppException("Username " + userParam.Username + " is already taken");
            //}

            //// update user properties
            //user.FirstName = userParam.FirstName;
            //user.LastName = userParam.LastName;
            //user.Username = userParam.Username;

            //// update password if it was entered
            //if (!string.IsNullOrWhiteSpace(password))
            //{
            //    byte[] passwordHash, passwordSalt;
            //    CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //    user.PasswordHash = passwordHash;
            //    user.PasswordSalt = passwordSalt;
            //}

            //_context.Users.Update(user);
            //_context.SaveChanges();
        }

        public void Delete(Guid userId)
        {
            var user = _uow.UserRepository.Get(x => x.UserID == userId);
            if(user==null)
            {
                throw new AppException(StringResources.UserDoesNotExist);
            }

            user.Deleted = true;
            _uow.UserRepository.Update(user);
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