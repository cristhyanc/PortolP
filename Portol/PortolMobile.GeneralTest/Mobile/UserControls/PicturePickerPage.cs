using Portol.Common;
using PortolMobile.Forms.ViewModels.UserControls;
using PortolMobile.GeneralTest.MockupServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Mobile.UserControls
{
    public class PicturePickerPage
    {

        [Fact]
        public void Take_photo_camera_no_available()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();

            CameraServiceMK cameraServiceMK = new CameraServiceMK();
            cameraServiceMK.IsCameraAvailable = false;
            PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(cameraServiceMK, navigationService, userDialogs);
            picturePickerViewModel.TakePhotoCommand.Execute(null);
          
            Assert.NotNull(userDialogs.UserDialogsArgs);
            Assert.Equal(StringResources.NoAvailableCamera, userDialogs.UserDialogsArgs.message);
        }

        [Fact]
        public void Take_photo_not_supported()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            CameraServiceMK cameraServiceMK = new CameraServiceMK();
            cameraServiceMK.IsCameraAvailable = true;
            cameraServiceMK.IsTakePhotoSupported = false;
            PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(cameraServiceMK, navigationService, userDialogs);
            picturePickerViewModel.TakePhotoCommand.Execute(null);

            Assert.NotNull(userDialogs.UserDialogsArgs);
            Assert.Equal(StringResources.NoAvailableCamera, userDialogs.UserDialogsArgs.message);

        }

        [Fact]
        public void Pickup_photo_not_supported()
        {
            NavigationServiceMK navigationService = new NavigationServiceMK();
            UserDialogsMK userDialogs = new UserDialogsMK();
            CameraServiceMK cameraServiceMK = new CameraServiceMK();          
            cameraServiceMK.IsPickPhotoSupported = false;
            PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(cameraServiceMK, navigationService, userDialogs);
            picturePickerViewModel.PickupPhotoCommand.Execute(null);

            Assert.NotNull(userDialogs.UserDialogsArgs);
            Assert.Equal(StringResources.NoPickPhotoSupported, userDialogs.UserDialogsArgs.message);

        }

        //[Fact]
        //public void Take_video_not_supported()
        //{
        //    NavigationServiceMK navigationService = new NavigationServiceMK();
        //    UserDialogsMK userDialogs = new UserDialogsMK();
        //    CameraServiceMK cameraServiceMK = new CameraServiceMK();
        //    cameraServiceMK.IsTakeVideoSupported = false;
        //    PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(cameraServiceMK, navigationService, userDialogs);
        //    picturePickerViewModel.TakeVideoCommand.Execute(null);

        //    Assert.NotNull(userDialogs.UserDialogsArgs);
        //    Assert.Equal(StringResources.NoTakeVideoSupported, userDialogs.UserDialogsArgs.message);

        //}

        //[Fact]
        //public void Pickup_video_not_supported()
        //{
        //    NavigationServiceMK navigationService = new NavigationServiceMK();
        //    UserDialogsMK userDialogs = new UserDialogsMK();
        //    CameraServiceMK cameraServiceMK = new CameraServiceMK();
        //    cameraServiceMK.IsPickVideoSupported = false;
        //    PicturePickerViewModel picturePickerViewModel = new PicturePickerViewModel(cameraServiceMK, navigationService, userDialogs);
        //    picturePickerViewModel.PickupVideoCommand.Execute(null);

        //    Assert.NotNull(userDialogs.UserDialogsArgs);
        //    Assert.Equal(StringResources.NoPickVideoSupported, userDialogs.UserDialogsArgs.message);

        //}

    }
}
