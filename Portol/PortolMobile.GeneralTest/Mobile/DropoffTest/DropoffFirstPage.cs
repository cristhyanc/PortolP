using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms;
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


        LoginServiceMK _loginServiceMK = null;
        SessionDataMK _sessionData = null;
        CustomerMobileServiceMK _mobileServiceMK = null;
        NavigationServiceMK _navigationService = null;
        UserDialogsMK _userDialogs = null;

        private void InitServices()
        {
            _loginServiceMK = new LoginServiceMK(uow);
            _sessionData = new SessionDataMK();
            _sessionData.LoginUser(_loginServiceMK, "", "").Wait();
            _mobileServiceMK = new CustomerMobileServiceMK(uow);
            _navigationService = new NavigationServiceMK();
            _userDialogs = new UserDialogsMK();
        }

        [Fact]        
        public async void GetCustomer_Validations( )
        {
            try
            {
                InitServices();


                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.MobileNumberEmailRequiered, _userDialogs.UserDialogsArgs.message );

                dropViewmodel.EmailMobileNumber = "asd";
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.MobileNumberEmailRequiered, _userDialogs.UserDialogsArgs.message);


                dropViewmodel.EmailMobileNumber = "040555555";
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.ReceiverNameRequired, _userDialogs.UserDialogsArgs.message);

                dropViewmodel.EmailMobileNumber = "asd@asd.com";
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.MissingInformation, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.ReceiverNameRequired, _userDialogs.UserDialogsArgs.message);

             
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
        public async void GetCustomer_PhoneExist(string name, string phone )
        {
            try
            {
                InitServices();

                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;               
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), _navigationService.viewModel);
                Assert.NotNull(_navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), _navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)_navigationService.Parameter;
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
        [InlineData("Peter",  "cristhyan@outlook.com")]
        [InlineData("Paulo",  "sed.tortor.Integer@nuncrisusvarius.org")]
        [InlineData("Mario",  "nec.diam@adipiscingelitCurabitur.edu")]
        public async void GetCustomer_Email(string name,  string email)
        {
            try
            {
                InitServices();

                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
             

                await dropViewmodel.GotoAddressStep();               

                Assert.NotNull(_navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), _navigationService.viewModel);
                Assert.NotNull(_navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), _navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)_navigationService.Parameter;           
                Assert.Equal(email, customer.Receiver.Email);
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
        public async void GetCustomer_Phone_NoExist_Email_Empty_AsGuess(string name, string phone )
        {
            try
            {

                InitServices();

                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);

                dropViewmodel.EmailMobileNumber = phone;
                dropViewmodel.ReceiverName = name;              

                _userDialogs.QuestionAnswer = true;
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, _userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, _userDialogs.UserDialogsArgs.okText);


                Assert.NotNull(_navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), _navigationService.viewModel);
                Assert.NotNull(_navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), _navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)_navigationService.Parameter;
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
        [InlineData("Peter",  "cristhyan@msn.com")]
        [InlineData("Paulo",  "Inxteger@nuncrisusvarius.org")]
        [InlineData("Mario",  "diam@xadipiscingelitCurabitur.edu")]
        public async void GetCustomer_Email_NoExist_AsGuess(string name,  string email )
        {
            try
            {
                InitServices();

                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;
              
                _userDialogs.QuestionAnswer = true;
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, _userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, _userDialogs.UserDialogsArgs.okText);
               

                Assert.NotNull(_navigationService.viewModel);
                Assert.Equal(typeof(DropAddressViewModel), _navigationService.viewModel);
                Assert.NotNull(_navigationService.Parameter);
                Assert.Equal(typeof(DropoffDto), _navigationService.Parameter.GetType());

                DropoffDto customer = (DropoffDto)_navigationService.Parameter;
                Assert.Equal(email, customer.Receiver.Email);
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
        public async void GetCustomer_Phone_NoExist_Email_NoExist_NoGuess(string name, string email )
        {
            try
            {
                InitServices();

                DropViewModel dropViewmodel = new DropViewModel(_mobileServiceMK, _navigationService, _userDialogs, _sessionData);

                dropViewmodel.EmailMobileNumber = email;
                dropViewmodel.ReceiverName = name;              

                _userDialogs.QuestionAnswer = false;
                await dropViewmodel.GotoAddressStep();

                Assert.NotNull(_userDialogs.UserDialogsArgs);
                Assert.Equal(StringResources.Guess, _userDialogs.UserDialogsArgs.title);
                Assert.Equal(StringResources.PersonNoRegistered, _userDialogs.UserDialogsArgs.message);
                Assert.Equal(StringResources.ContinueGuess, _userDialogs.UserDialogsArgs.okText);

                Assert.Null(_navigationService.viewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
