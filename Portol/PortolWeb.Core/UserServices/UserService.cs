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

        public UserDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AppException("Email or Password is incorrect");               
            }
                

            var user = _uow.UserRepository.Get(x => x.Email == username);                      
            if (user == null)
            {
                throw new AppException("Email or Password is incorrect");
            }               
                      
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException("Email or Password is incorrect");
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
                throw new AppException("DOB Required");
            }

            if(string.IsNullOrEmpty(newUser.Email ))
            {
                throw new AppException("Email Required");
            }

            if (string.IsNullOrEmpty(newUser.FirstName))
            {
                throw new AppException("FirstName Required");
            }

            if (string.IsNullOrEmpty(newUser.LastName))
            {
                throw new AppException("LastName Required");
            }

            if (newUser.PhoneNumber==0)
            {
                throw new AppException("PhoneNumber Required");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new AppException("Password Required");
            }


            if (_uow.UserRepository.Get(x => x.Email == newUser.Email) != null)
            {
                throw new AppException("The Email \"" + newUser.Email + "\" is already being used");
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
                throw new AppException("User Does not Exist");
            }

            user.Deleted = true;
            _uow.UserRepository.Update(user);
            _uow.SaveChanges();
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new AppException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new AppException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new AppException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new AppException("Value cannot be empty or whitespace only string.", "password");
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