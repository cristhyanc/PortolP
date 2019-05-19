﻿using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
    public interface IUserService
    {
        UserDto Authenticate(string username, string password);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(Guid userId);
        UserDto Create(UserDto user, string password);
        void Update(UserDto user, string password = null);
        void Delete(Guid userId);
        void ResetPassword(UserDto user);
        bool VerifyMobileUniqueness(UserDto phoneDetails);
        bool VerifyEmailUniqueness(string email);
        bool ValidateVerificationCode(long phoneNumber, Int32 countryCode, Int32 code);

    }
}