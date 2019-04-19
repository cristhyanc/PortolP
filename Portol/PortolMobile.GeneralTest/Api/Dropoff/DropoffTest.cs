using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Core.DropoffServices;
using PortolWeb.Core.UserServices;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Api.Dropoff
{
    [Collection("Database collection")]
    public class DropoffTest
    {
        IUnitOfWork uow;
        public DropoffTest(DatabaseFixture fixture)
        {
            uow = fixture.UnitOfWorkDB;
        }

        [Theory]
        [InlineData(22998887, 24)]
        [InlineData(64732149, 23)]
        [InlineData(96009533, 50)]
        [InlineData(25014561, 32)]
        public void GetCustomerByPhoneNumber_Passed(long phone, Int32 countryCode)
        {

            ICustomerService dropoff = new CustomerService(uow);
            var customer = dropoff.GetCustomerByPhoneNumber(phone, countryCode);
            Assert.NotNull(customer);
        }

        [Theory]
        [InlineData(0405593358, 61)]
        [InlineData(0405593355, 61)]
        [InlineData(0405593354, 61)]
        [InlineData(0405593353, 61)]
        public void GetCustomerByPhoneNumber_DontExist(long phone, Int32 countryCode)
        {

            ICustomerService dropoff = new CustomerService(uow);
            var customer = dropoff.GetCustomerByPhoneNumber(phone, countryCode);
            Assert.Null(customer);
        }
    }
}
