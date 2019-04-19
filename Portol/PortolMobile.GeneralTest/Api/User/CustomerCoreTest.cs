using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Core.UserServices;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Api.User
{
    [Collection("Database collection")]
    public class CustomerCoreTest 
    {
        IUnitOfWork uow;
        public CustomerCoreTest(DatabaseFixture fixture)
        {
            uow = fixture.UnitOfWorkDB;
        }

        [Theory]
        [InlineData(5949849170, 83, 3804)]
        [InlineData(3114119227, 79, 7155)]
        [InlineData(7264971097, 8, 2710)]
        [InlineData(6443024302, 30, 3899)]
        public void ValidateVerificationCode_CorrectCode(long phoneNumber, Int32 countryCode, Int32 code)
        {
            ICustomerService customeSer = new CustomerService(uow);
            Assert.True( customeSer.ValidateVerificationCode(phoneNumber, countryCode, code));
            
        }

        [Theory]
        [InlineData(5949849170, 83, 3404)]
        [InlineData(3114119827, 79, 7155)]
        [InlineData(7264971297, 8, 2710)]
        [InlineData(2742042813, 25, 9874)]
        public void ValidateVerificationCode_WrongCode(long phoneNumber, Int32 countryCode, Int32 code)
        {
            ICustomerService customeSer = new CustomerService(uow);
            Assert.False(customeSer.ValidateVerificationCode(phoneNumber, countryCode, code));

        }

    }
}
