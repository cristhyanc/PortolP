using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels;
using PortolMobile.Forms.ViewModels.Dropoff;
using PortolMobile.GeneralTest.Api;
using PortolMobile.GeneralTest.MockupServices;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Mobile.DropoffTest
{
    [Collection("Database collection")]
    public class DropoffFirstPage
    {
        IUnitOfWork uow;
        public DropoffFirstPage(DatabaseFixture fixture)
        {
            uow = fixture.UnitOfWorkDB;
        }

        [Fact]        
        public async void GetCustomer_Validations( )
        {
            try
            {
                ICustomerMobileService mobileServiceMK = null;
                INavigationService navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.MobileNumberEmailRequiered, userDialogs.UserDialogsArgs.message );

                dropViewmodel.EmailMobileNumber = "asd";
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.MobileNumberEmailRequiered, userDialogs.UserDialogsArgs.message);


                dropViewmodel.EmailMobileNumber = "040555555";
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.ReceiverNameRequired, userDialogs.UserDialogsArgs.message);

                dropViewmodel.EmailMobileNumber = "asd@asd.com";
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.ReceiverNameRequired, userDialogs.UserDialogsArgs.message);

                dropViewmodel.ReceiverName = "cris";
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.DescriptionRequired, userDialogs.UserDialogsArgs.message);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }



        [Theory ]
        [InlineData("Peter", "91579496","asd")]
        [InlineData("Paulo", "10675693", "111")]
        [InlineData("Mario", "91344725", "ttt")]
        public async void GetCustomer_PhoneExist(string name, string phone, string description)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;
                dropViewmodel.Description  = description;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)navigationService.Parameter;
                long phoneN = 0;
                long.TryParse(phone, out phoneN);
                Assert.Equal(phoneN, customer.Receiver.PhoneNumber);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Theory]
        [InlineData("Peter",  "cristhyan@outlook.com", "asd")]
        [InlineData("Paulo",  "sed.tortor.Integer@nuncrisusvarius.org", "bbb")]
        [InlineData("Mario",  "nec.diam@adipiscingelitCurabitur.edu", "333")]
        public async void GetCustomer_Email(string name,  string email, string description)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
                dropViewmodel.Description = description;

                await dropViewmodel.GetCustomer();               

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)navigationService.Parameter;           
                Assert.Equal(email, customer.Receiver.Email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter", "11111111", "asd")]
        [InlineData("Paulo", "22222222", "asd")]
        [InlineData("Mario", "33333333", "asd")]
        public async void GetCustomer_Phone_NoExist_Email_Empty_AsGuess(string name, string phone, string description)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;
                dropViewmodel.Description = description;

                userDialogs.QuestionAnswer = true;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, userDialogs.UserDialogsArgs.okText);


                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)navigationService.Parameter;
                long phoneN = 0;
                long.TryParse(phone, out phoneN);
                Assert.Equal(phoneN, customer.Receiver.PhoneNumber);
                Assert.Equal(StringResources.GuessEmail , customer.Receiver.Email );
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter",  "cristhyan@msn.com", "asd")]
        [InlineData("Paulo",  "Inxteger@nuncrisusvarius.org", "asd")]
        [InlineData("Mario",  "diam@xadipiscingelitCurabitur.edu", "asd")]
        public async void GetCustomer_Email_NoExist_AsGuess(string name,  string email, string description)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
                dropViewmodel.Description = description;

                userDialogs.QuestionAnswer = true;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, userDialogs.UserDialogsArgs.okText);
               

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)navigationService.Parameter;
                Assert.Equal(email, customer.Receiver.Email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter", "cristhyan@msn.com", "asd")]
        [InlineData("Paulo", "Inxteger@nuncrisusvarius.org", "asd")]
        [InlineData("Mario", "diam@xadipiscingelitCurabitur.edu", "asd")]
        public async void GetCustomer_Phone_NoExist_Email_NoExist_NoGuess(string name, string email, string description)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
                dropViewmodel.Description = description;

                userDialogs.QuestionAnswer = false;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, userDialogs.UserDialogsArgs.okText);

                Assert.Null(navigationService.viewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
