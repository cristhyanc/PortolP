//using Portol.Common;
//using Portol.Common.DTO;
//using Portol.Common.Helper;
//using PortolMobile.Forms.Services.Navigation;
//using PortolMobile.Forms.ViewModels.Dropoff;
//using PortolMobile.Forms.ViewModels.UserControls;
//using PortolMobile.GeneralTest.Api;
//using PortolMobile.GeneralTest.MockupServices;
//using PortolWeb.Entities;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace PortolMobile.GeneralTest.Mobile.DropoffTest
//{
//    [Collection("Database collection")]
//    public   class DropoffAddressPage
//    {
//        IUnitOfWork uow;
//        public DropoffAddressPage(DatabaseFixture fixture)
//        {
//            uow = fixture.UnitOfWorkDB;
//        }

//        [Theory]
//        [InlineData("71 arthur st", "fortitude valley", "4006")]
//        [InlineData("79 berwick st", "fortitude valley", "4006")]
//        [InlineData("84 sydney st", "New farm", "4005")]
//        public async void GotoPicturesPage_Validations(string street, string suburb, string postcode)
//        {
//            try
//            {
//                AddressDto addressDto = new AddressDto();
//                addressDto.AddressValidated = true;
//                addressDto.PostCode = postcode;
//                addressDto.Suburb = suburb;
//                addressDto.StreetName = street;
//                DeliveryDto dropoffDto = new DeliveryDto();

//                 NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs,null);

//                await dropViewmodel.InitializeAsync(dropoffDto);

//                await dropViewmodel.GotoPicturesPage();
//                Assert.NotNull(userDialogs.UserDialogsArgs);

//                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
//                Assert.Equal(StringResources.PickupAddressRequired, userDialogs.UserDialogsArgs.message);

//                dropViewmodel.PickUpAddress = addressDto;
//                await dropViewmodel.GotoPicturesPage();

//                Assert.NotNull(userDialogs.UserDialogsArgs);
//                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
//                Assert.Equal(StringResources.DropoffAddressRequired, userDialogs.UserDialogsArgs.message);

//                dropViewmodel.DropoffAddress  = addressDto;
//                await dropViewmodel.GotoPicturesPage();

//                Assert.NotNull(userDialogs.UserDialogsArgs);
//                Assert.Equal(StringResources.MissingInformation, userDialogs.UserDialogsArgs.title);
//                Assert.Equal(StringResources.DescriptionRequired, userDialogs.UserDialogsArgs.message);

//                dropViewmodel.Description = "cris";
//                await dropViewmodel.GotoPicturesPage();

//                Assert.NotNull(navigationService.viewModel);
//                Assert.Equal(typeof(DropPicturesViewModel), navigationService.viewModel);
//                Assert.NotNull(navigationService.Parameter);
//                Assert.Equal(typeof(DeliveryDto), navigationService.Parameter.GetType());

               

               
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }


//        [Fact]
//        public async void GotoAddressPage_PickupAddress_EmptyAddress()
//        {
//            try
//            {

//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);
//                await dropViewmodel.GotoAddressPage("pickup");
                
//                Assert.NotNull(navigationService.viewModel);
//                Assert.Equal(typeof(AddressPickerViewModel), navigationService.viewModel);
//                Assert.NotNull(navigationService.Parameter);
//                Assert.Equal(typeof(AddressPickerParameters), navigationService.Parameter.GetType());

//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;

//                Assert.True(parameter.IsPickupAddress);
//                Assert.Null(parameter.Address);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        [Theory]
//        [InlineData("71 arthur st", "fortitude valley", "4006")]
//        [InlineData("79 berwick st", "fortitude valley", "4006")]
//        [InlineData("84 sydney st", "New farm", "4005")]
//        public async void GotoAddressPage_PickupAddress_Address(string street, string suburb, string postcode)
//        {
//            try
//            {

//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);

//                AddressDto addressDto = new AddressDto();                               
//                addressDto.AddressValidated = true;
//                addressDto.PostCode = postcode;
//                addressDto.Suburb = suburb;
//                addressDto.StreetName = street;

//                dropViewmodel.PickUpAddress = addressDto;

//                await dropViewmodel.GotoAddressPage("pickup");

//                Assert.NotNull(navigationService.viewModel);
//                Assert.Equal(typeof(AddressPickerViewModel), navigationService.viewModel);
//                Assert.NotNull(navigationService.Parameter);
//                Assert.Equal(typeof(AddressPickerParameters), navigationService.Parameter.GetType());

//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;

//                Assert.True(parameter.IsPickupAddress);
//                Assert.True(parameter.Address.AddressValidated);
//                Assert.Equal(street, parameter.Address.StreetName);
//                Assert.Equal(suburb, parameter.Address.Suburb);
//                Assert.Equal(postcode, parameter.Address.PostCode);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }


//        [Fact]
//        public async void GotoAddressPage_Dropoff_EmptyAddress()
//        {
//            try
//            {

//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);
//                await dropViewmodel.GotoAddressPage("dropoff");

//                Assert.NotNull(navigationService.viewModel);
//                Assert.Equal(typeof(AddressPickerViewModel), navigationService.viewModel);
//                Assert.NotNull(navigationService.Parameter);
//                Assert.Equal(typeof(AddressPickerParameters), navigationService.Parameter.GetType());

//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;

//                Assert.False (parameter.IsPickupAddress);
//                Assert.Null(parameter.Address);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        [Theory]
//        [InlineData("71 arthur st", "fortitude valley", "4006")]
//        [InlineData("79 berwick st", "fortitude valley", "4006")]
//        [InlineData("84 sydney st", "New farm", "4005")]
//        public async void GotoAddressPage_Dropoff_Address(string street, string suburb, string postcode)
//        {
//            try
//            {
//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);

//                AddressDto addressDto = new AddressDto();
//                addressDto.AddressValidated = true;
//                addressDto.PostCode = postcode;
//                addressDto.Suburb = suburb;
//                addressDto.StreetName = street;

//                dropViewmodel.DropoffAddress  = addressDto;

//                await dropViewmodel.GotoAddressPage("dropoff");

//                Assert.NotNull(navigationService.viewModel);
//                Assert.Equal(typeof(AddressPickerViewModel), navigationService.viewModel);
//                Assert.NotNull(navigationService.Parameter);
//                Assert.Equal(typeof(AddressPickerParameters), navigationService.Parameter.GetType());

//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;

//                Assert.False (parameter.IsPickupAddress);
//                Assert.True(parameter.Address.AddressValidated);
//                Assert.Equal(street, parameter.Address.StreetName);
//                Assert.Equal(suburb, parameter.Address.Suburb);
//                Assert.Equal(postcode, parameter.Address.PostCode);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }


//        [Theory]
//        [InlineData("71 arthur st", "fortitude valley", "4006")]
//        [InlineData("79 berwick st", "fortitude valley", "4006")]
//        [InlineData("84 sydney st", "New farm", "4005")]
//        public async void GotoAddressPage_pickupAddress_Address_from_AddressPicker(string street, string suburb, string postcode)
//        {
//            try
//            {
//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);
//                AddressPickerViewModel addressPickerView = new AddressPickerViewModel(navigationService, userDialogs, null);
//                bool gotAddress = false;

//                AddressDto addressDto = new AddressDto();
//                addressDto.AddressValidated = true;
//                addressDto.PostCode = postcode;
//                addressDto.Suburb = suburb;
//                addressDto.StreetName = street;

//                System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) => {

//                    if (e.PropertyName.Equals("PickUpAddress"))
//                    {
//                        DropAddressViewModel viewModel = (DropAddressViewModel)sender;
//                        Assert.Equal(street, viewModel.PickUpAddress.StreetName);
//                        Assert.Equal(suburb, viewModel.PickUpAddress.Suburb);
//                        Assert.Equal(postcode, viewModel.PickUpAddress.PostCode);

//                        gotAddress = true;
//                    }
//                };


//                dropViewmodel.PropertyChanged += handler;              
               
//                await dropViewmodel.GotoAddressPage("pickup");
//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;
//                parameter.Address = addressDto;

//                Xamarin.Forms.MessagingCenter.Send<AddressPickerViewModel, AddressPickerParameters>(addressPickerView, MessagingCenterCodes.AddressPickerMessage, parameter);

//                dropViewmodel.PropertyChanged -= handler;
//                Assert.True(gotAddress);

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        [Theory]
//        [InlineData("71 arthur st", "fortitude valley", "4006")]
//        [InlineData("79 berwick st", "fortitude valley", "4006")]
//        [InlineData("84 sydney st", "New farm", "4005")]
//        public async void GotoAddressPage_DropoofAddress_Address_from_AddressPicker(string street, string suburb, string postcode)
//        {
//            try
//            {
//                NavigationServiceMK navigationService = new NavigationServiceMK();
//                UserDialogsMK userDialogs = new UserDialogsMK();
//                DropAddressViewModel dropViewmodel = new DropAddressViewModel(navigationService, userDialogs, null);
//                AddressPickerViewModel addressPickerView = new AddressPickerViewModel(navigationService, userDialogs, null);
//                bool gotAddress = false;

//                AddressDto addressDto = new AddressDto();
//                addressDto.AddressValidated = true;
//                addressDto.PostCode = postcode;
//                addressDto.Suburb = suburb;
//                addressDto.StreetName = street;

//                System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) => {

//                    if (e.PropertyName.Equals("DropoffAddress"))
//                    {
//                        DropAddressViewModel viewModel = (DropAddressViewModel)sender;
//                        Assert.Equal(street, viewModel.DropoffAddress.StreetName);
//                        Assert.Equal(suburb, viewModel.DropoffAddress.Suburb);
//                        Assert.Equal(postcode, viewModel.DropoffAddress.PostCode);

//                        gotAddress = true;
//                    }
//                };


//                dropViewmodel.PropertyChanged += handler;

//                await dropViewmodel.GotoAddressPage("dropoof");
//                AddressPickerParameters parameter = (AddressPickerParameters)navigationService.Parameter;
//                parameter.Address = addressDto;

//                Xamarin.Forms.MessagingCenter.Send<AddressPickerViewModel, AddressPickerParameters>(addressPickerView, MessagingCenterCodes.AddressPickerMessage, parameter);

//                dropViewmodel.PropertyChanged -= handler;
//                Assert.True(gotAddress);

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//    }
//}
