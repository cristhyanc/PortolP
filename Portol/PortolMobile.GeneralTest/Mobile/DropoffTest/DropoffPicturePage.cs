using Portol.Common.DTO;
using PortolMobile.Forms.ViewModels.Dropoff;
using PortolMobile.Forms.ViewModels.UserControls;
using PortolMobile.GeneralTest.MockupServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace PortolMobile.GeneralTest.Mobile.DropoffTest
{
    public class DropoffPicturePage
    {
        [Fact]
        public async void Open_PicturePickerView()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(navigationService, userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);

            EventHandler handler = (sender, e) =>
            {

                NavigationServiceMK viewModel = (NavigationServiceMK)sender;

                Assert.NotNull(viewModel.viewModel);
                Assert.Equal(typeof(PicturePickerViewModel), viewModel.viewModel);
                Assert.Equal(parameter, viewModel.Parameter);
            };
            navigationService.NavigationCalled += handler;

            dropPicturesViewModel.PickPicturesCommand.Execute(null);

            navigationService.NavigationCalled += handler;

        }

        [Fact]
        public async void Open_MeasurementView()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(navigationService, userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);

            EventHandler handler = (sender, e) =>
            {
                NavigationServiceMK viewModel = (NavigationServiceMK)sender;

                Assert.NotNull(viewModel.viewModel);
                Assert.Equal(typeof(DropMeasurementsViewModel), viewModel.viewModel);
                Assert.Equal(parameter, viewModel.Parameter);
            };

            navigationService.NavigationCalled += handler;
            dropPicturesViewModel.MeasurementCommand.Execute(null);
            navigationService.NavigationCalled += handler;

        }

        [Theory]
        [InlineData((object)(new object[] { "url11", "url22" }))]
        [InlineData((object)(new object[] { "xxxx", "yyy","asdasd" }))]
        [InlineData((object)(new object[] { "xxxx" }))]
        public async void GallerySection_Visible(params string[] urls)
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(navigationService, userDialogs);
            
            DropoffDto parameter = new DropoffDto();

            parameter.Images = urls.Select(x => new PicturesDto { ImageUrl = x }).ToList(); 
            await dropPicturesViewModel.InitializeAsync(parameter);

            Assert.True(dropPicturesViewModel.IsGalleryVisible);
            Assert.False(dropPicturesViewModel.IsPicturePickerButtonVisible);
        }

        [Fact]
        public async void PicturePickerSection_Visible()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(navigationService, userDialogs);

            DropoffDto parameter = new DropoffDto(); 
            await dropPicturesViewModel.InitializeAsync(parameter);

            Assert.False(dropPicturesViewModel.IsGalleryVisible);
            Assert.True(dropPicturesViewModel.IsPicturePickerButtonVisible);
        }
    }
}
