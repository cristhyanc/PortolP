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
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }



        [Theory ]
        [InlineData("Peter", "91579496")]
        [InlineData("Paulo", "10675693")]
        [InlineData("Mario", "91344725")]
        public async void GetCustomer_PhoneExist(string name, string phone)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;

                await dropViewmodel.GetCustomer();

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(CustomerDto), navigationService.Parameter.GetType());
                               
                CustomerDto customer = (CustomerDto)navigationService.Parameter;
                long phoneN = 0;
                long.TryParse(phone, out phoneN);
                Assert.Equal(phoneN, customer.PhoneNumber);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Theory]
        [InlineData("Peter",  "cristhyan@outlook.com")]
        [InlineData("Paulo",  "sed.tortor.Integer@nuncrisusvarius.org")]
        [InlineData("Mario",  "nec.diam@adipiscingelitCurabitur.edu")]
        public async void GetCustomer_Email(string name,  string email)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;               
               
                await dropViewmodel.GetCustomer();               

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(CustomerDto), navigationService.Parameter.GetType());

                CustomerDto customer = (CustomerDto)navigationService.Parameter;           
                Assert.Equal(email, customer.Email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter", "11111111")]
        [InlineData("Paulo", "22222222")]
        [InlineData("Mario", "33333333")]
        public async void GetCustomer_Phone_NoExist_Email_Empty_AsGuess(string name, string phone)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;               
                userDialogs.QuestionAnswer = true;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, userDialogs.UserDialogsArgs.okText);


                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(CustomerDto), navigationService.Parameter.GetType());

                CustomerDto customer = (CustomerDto)navigationService.Parameter;
                long phoneN = 0;
                long.TryParse(phone, out phoneN);
                Assert.Equal(phoneN, customer.PhoneNumber);
                Assert.Equal(StringResources.GuessEmail , customer.Email );
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter",  "cristhyan@msn.com")]
        [InlineData("Paulo",  "Inxteger@nuncrisusvarius.org")]
        [InlineData("Mario",  "diam@xadipiscingelitCurabitur.edu")]
        public async void GetCustomer_Email_NoExist_AsGuess(string name,  string email)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;              
                userDialogs.QuestionAnswer = true;
                await dropViewmodel.GetCustomer();

                Assert.NotNull(userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, userDialogs.UserDialogsArgs.okText);
               

                Assert.NotNull(navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), navigationService.viewModel);
                Assert.NotNull(navigationService.Parameter);
                Assert.Equal(typeof(CustomerDto), navigationService.Parameter.GetType());

                CustomerDto customer = (CustomerDto)navigationService.Parameter;
                Assert.Equal(email, customer.Email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Theory]
        [InlineData("Peter", "cristhyan@msn.com")]
        [InlineData("Paulo", "Inxteger@nuncrisusvarius.org")]
        [InlineData("Mario", "diam@xadipiscingelitCurabitur.edu")]
        public async void GetCustomer_Phone_NoExist_Email_NoExist_NoGuess(string name, string email)
        {
            try
            {
                CustomerMobileServiceMK mobileServiceMK = new CustomerMobileServiceMK(uow);
                NavigationServiceMK navigationService = new NavigationServiceMK();
                UserDialogsMK userDialogs = new UserDialogsMK();
                DropViewModel dropViewmodel = new DropViewModel(mobileServiceMK, navigationService, userDialogs);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
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
