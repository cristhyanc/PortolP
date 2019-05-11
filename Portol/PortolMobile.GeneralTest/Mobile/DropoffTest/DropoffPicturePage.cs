using Portol.Common.DTO;
using PortolMobile.Forms.ViewModels.Dropoff;
using PortolMobile.Forms.ViewModels.UserControls;
using PortolMobile.GeneralTest.MockupServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Xamarin.Forms;
using Portol.Common.Helper;
using Portol.Common;

namespace PortolMobile.GeneralTest.Mobile.DropoffTest
{
    public class DropoffPicturePage
    {
        NavigationServiceMK _navigationService = null;
        UserDialogsMK _userDialogs = null;
        private void InitServices()
        {
             _navigationService = new NavigationServiceMK();
             _userDialogs = new UserDialogsMK();
        }


        [Fact]
        public async void Open_PicturePickerView()
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);

            EventHandler handler = (sender, e) =>
            {

                NavigationServiceMK viewModel = (NavigationServiceMK)sender;

                Assert.NotNull(viewModel.viewModel);
                Assert.Equal(typeof(PicturePickerViewModel), viewModel.viewModel);
                Assert.Equal(parameter, viewModel.Parameter);
            };
            _navigationService.NavigationCalled += handler;

            dropPicturesViewModel.PickPicturesCommand.Execute(null);

            _navigationService.NavigationCalled += handler;

        }

        [Fact]
        public async void Open_MeasurementView()
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);

            EventHandler handler = (sender, e) =>
            {
                NavigationServiceMK viewModel = (NavigationServiceMK)sender;

                Assert.NotNull(viewModel.viewModel);
                Assert.Equal(typeof(DropMeasurementsViewModel), viewModel.viewModel);
                Assert.Equal(parameter, viewModel.Parameter);
            };

            _navigationService.NavigationCalled += handler;
            dropPicturesViewModel.MeasurementCommand.Execute(null);
            _navigationService.NavigationCalled += handler;

        }

        [Theory]
        [InlineData((object)(new object[] { "url11", "url22" }))]
        [InlineData((object)(new object[] { "xxxx", "yyy","asdasd" }))]
        [InlineData((object)(new object[] { "xxxx" }))]
        public async void GallerySection_Visible(params string[] urls)
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);
            
            DropoffDto parameter = new DropoffDto();

            parameter.Images = urls.Select(x => new PicturesDto { ImageUrl = x }).ToList(); 
            await dropPicturesViewModel.InitializeAsync(parameter);

            Assert.True(dropPicturesViewModel.IsGalleryVisible);
            Assert.False(dropPicturesViewModel.IsPicturePickerButtonVisible);
        }

        [Fact]
        public async void PicturePickerSection_Visible()
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);

            DropoffDto parameter = new DropoffDto(); 
            await dropPicturesViewModel.InitializeAsync(parameter);

            Assert.False(dropPicturesViewModel.IsGalleryVisible);
            Assert.True(dropPicturesViewModel.IsPicturePickerButtonVisible);
        }

        [Fact]
        public async void GetPictures_From_PicturePicker()
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);
            PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(null, _navigationService, _userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);
            dropPicturesViewModel.PickPicturesCommand.Execute(null);

            List<PicturesDto> pictures = new List<PicturesDto>();
            pictures.Add(new PicturesDto());
            pictures.Add(new PicturesDto());
            pictures.Add(new PicturesDto());

            picturePickerViewModel.Pictures = new System.Collections.ObjectModel.ObservableCollection<PicturesDto>(pictures);

            System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) => {

                if (e.PropertyName.Equals("ImagesTaken"))
                {
                    DropPicturesViewModel viewModel = (DropPicturesViewModel)sender;
                    Assert.Equal(dropPicturesViewModel.ImagesTaken, pictures);                  
                }
                if (e.PropertyName.Equals("IsGalleryVisible"))
                {
                    DropPicturesViewModel viewModel = (DropPicturesViewModel)sender;
                    Assert.True(dropPicturesViewModel.IsGalleryVisible);
                    Assert.False(dropPicturesViewModel.IsPicturePickerButtonVisible);
                }
            };

            dropPicturesViewModel.PropertyChanged += handler;
            picturePickerViewModel.DoneCommand.Execute(null);
            dropPicturesViewModel.PropertyChanged -= handler;

        }


        [Theory]
        [InlineData(5, 24, 35, 222)]
        [InlineData(105, 26, 3, 25)]
        [InlineData(500, 278, 366, 12)]
        public async void GetMesurements(int width, int height,int length, int weight)
        {
            InitServices();
            DropPicturesViewModel dropPicturesViewModel = new DropPicturesViewModel(_navigationService, _userDialogs);
            DropMeasurementsViewModel dropMeasurementsViewModel =new DropMeasurementsViewModel(_navigationService, _userDialogs);
            DropoffDto parameter = new DropoffDto();
            await dropPicturesViewModel.InitializeAsync(parameter);
            dropPicturesViewModel.MeasurementCommand.Execute(null);

            MeasurementDto measurement = new MeasurementDto();
            measurement.Height = height;
            measurement.Length = length;
            measurement.Weight = weight;
            measurement.Width = width;

            dropMeasurementsViewModel.Measurements = measurement;

            System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) => {

                if (e.PropertyName.Equals("ParcelVolume"))
                {
                    DropPicturesViewModel viewModel = (DropPicturesViewModel)sender;
                    Assert.Equal(dropPicturesViewModel.ParcelVolume , ((width* height* length).ToString() + " " + StringResources.M3));
                }
               
            };
           
            dropPicturesViewModel.PropertyChanged += handler;
            dropMeasurementsViewModel.GoBackToPicturesCommand.Execute(null);
            // MessagingCenter.Send<DropMeasurementsViewModel, MeasurementDto>(dropMeasurementsViewModel, MessagingCenterCodes.MeasurementMessage, measurement);
            dropPicturesViewModel.PropertyChanged -= handler;

        }
    }
}
