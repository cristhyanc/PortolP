//using Portol.Common.Interfaces.PortolWeb;
//using PortolWeb.Core.UserServices;
//using PortolWeb.Entities;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace PortolMobile.GeneralTest.Api.User
//{
//    [Collection("Database collection")]
//    public class CustomerCoreTest 
//    {
//        IUnitOfWork uow;
//        public CustomerCoreTest(DatabaseFixture fixture)
//        {
//            uow = fixture.UnitOfWorkDB;
//        }

//        [Theory]
//        [InlineData(5949849170, 83, 3804)]
//        [InlineData(3114119227, 79, 7155)]
//        [InlineData(7264971097, 8, 2710)]
//        [InlineData(6443024302, 30, 3899)]
//        public void ValidateVerificationCode_CorrectCode(long phoneNumber, Int32 countryCode, Int32 code)
//        {
//            ICustomerService customeSer = new CustomerService(uow);
//            Assert.True( customeSer.ValidateVerificationCode(phoneNumber, countryCode, code));
            
//        }

//        [Theory]
//        [InlineData(5949849170, 83, 3404)]
//        [InlineData(3114119827, 79, 7155)]
//        [InlineData(7264971297, 8, 2710)]
//        [InlineData(2742042813, 25, 9874)]
//        public void ValidateVerificationCode_WrongCode(long phoneNumber, Int32 countryCode, Int32 code)
//        {
//            ICustomerService customeSer = new CustomerService(uow);
//            Assert.False(customeSer.ValidateVerificationCode(phoneNumber, countryCode, code));

//        }

//        [Theory]
//        [InlineData(22998887, 24)]
//        [InlineData(64732149, 23)]
//        [InlineData(96009533, 50)]
//        [InlineData(25014561, 32)]
//        public void GetCustomerByPhoneNumber_Passed(long phone, Int32 countryCode)
//        {

//            ICustomerService dropoff = new CustomerService(uow);
//            var customer = dropoff.GetCustomerByPhoneNumber(phone, countryCode);
//            Assert.NotNull(customer);
//        }

//        [Theory]
//        [InlineData(0405593357, 61)]
//        [InlineData(0405593355, 61)]
//        [InlineData(0405593354, 61)]
//        [InlineData(0405593353, 61)]
//        public void GetCustomerByPhoneNumber_DontExist(long phone, Int32 countryCode)
//        {

//            ICustomerService dropoff = new CustomerService(uow);
//            var customer = dropoff.GetCustomerByPhoneNumber(phone, countryCode);
//            Assert.Null(customer);
//        }


//        [Theory]
//        [InlineData("cristhyan3@outlook.com")]
//        [InlineData("cristhyanc@gmail.com")]
//        [InlineData("cristhyan1@outlook.com")]
//        [InlineData("cristhyan2@outlook.com")]
//        public void GetCustomerByEmail_DontExist(string email)
//        {

//            ICustomerService dropoff = new CustomerService(uow);
//            var customer = dropoff.GetCustomerByEmail(email);
//            Assert.Null(customer);
//        }


//        [Theory]
//        [InlineData("pede.Praesent.eu@dui.org")]
//        [InlineData("sem.Pellentesque.ut@Sed.net")]
//        [InlineData("nec.diam@adipiscingelitCurabitur.edu")]
//        [InlineData("sed.tortor.Integer@nuncrisusvarius.org")]
//        public void GetCustomerByEmail_Exist(string email)
//        {

//            ICustomerService dropoff = new CustomerService(uow);
//            var customer = dropoff.GetCustomerByEmail(email);
//            Assert.NotNull(customer);
//        }
//    }
//}
