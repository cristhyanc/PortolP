﻿using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Images;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.UserControls
{
    public class PicturePickerParameters
    {
        public List<PictureDto> Pictures { get; set; }
        public int? MaximunPicturesAllowed { get; set; }
    }
    public class PicturePickerViewModel : BaseViewModel
    {


        public ICommand TakePhotoCommand { get; private set; }
        public ICommand PickupVideoCommand { get; private set; }
        public ICommand TakeVideoCommand { get; private set; }
        public ICommand PickupPhotoCommand { get; private set; }
        public ICommand SelectedPhotoCommand { get; private set; }
        public ICommand DoneCommand { get; private set; }
        public ICommand DeletePhotoCommand { get; private set; }


        PictureDto _selectedPicture;
        public PictureDto SelectedPicture
        {
            get
            {
                return _selectedPicture;
            }
            set
            {
                _selectedPicture = value;
                if (value == null)
                {
                    IsDeleteButtonVisible = false;
                }
                else
                {
                    IsDeleteButtonVisible = true;
                }
                OnPropertyChanged();
            }
        }

        

        bool  _isDeleteButtonVisible;
        public bool IsDeleteButtonVisible
        {
            get
            {
                return _isDeleteButtonVisible;
            }
            set
            {
                _isDeleteButtonVisible = value;
                OnPropertyChanged();
            }

        }

        ObservableCollection<PictureDto> _pictures;
        public ObservableCollection<PictureDto> Pictures
        {
            get
            {
                return _pictures;
            }
            set
            {
                _pictures = value;
                OnPropertyChanged();
            }

        }

        private int? MaximunPicturesAllowed;

        IMedia _media;
        public PicturePickerViewModel(IMedia media, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            _media = media;
            TakePhotoCommand = new Command(TakePhoto, () => { return !IsBusy; });
            PickupPhotoCommand = new Command(PickupPhoto, () => { return !IsBusy; });
            SelectedPhotoCommand = new Command<Guid>(DisplayPhotoSelected, (x) => { return !IsBusy; });
            DoneCommand = new Command(GoBack, () => { return !IsBusy; });
            DeletePhotoCommand = new Command(DeletePhoto, () => { return !IsBusy; });
            Pictures = new ObservableCollection<PictureDto>();
        }


        private void DeletePhoto()
        {
            try
            {
                this.IsBusy = true;
                if(SelectedPicture != null)
                {
                    this.Pictures.Remove(SelectedPicture);
                    SelectedPicture = null;
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "InitializeAsync");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                if (navigationData != null)
                {
                    
                    PicturePickerParameters picturePicker = (PicturePickerParameters)navigationData;
                    List<PictureDto> picturesDtos = picturePicker.Pictures;
                    if (picturesDtos != null)
                    {
                        Pictures = new ObservableCollection<PictureDto>(picturesDtos);
                        SelectedPicture = Pictures.FirstOrDefault();
                    }
                    else
                    {
                        Pictures = new ObservableCollection<PictureDto>();
                        TakePhoto();
                    }
                    this.MaximunPicturesAllowed = picturePicker.MaximunPicturesAllowed;

                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "InitializeAsync");
            }
            finally
            {
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }

        private async void GoBack()
        {
            try
            {
                if (this.IsBusy)
                {
                    return;
                }
                this.IsBusy = true;
                MessagingCenter.Send<PicturePickerViewModel, List<PictureDto>>(this, MessagingCenterCodes.PicturePickerMessage, this.Pictures.ToList());
                await this.NavigationService.GoToPreviousPageAsync();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "GoBack");
                this.IsBusy = false;
            }
           
        }

        private void DisplayPhotoSelected(Guid pictureId)
        {
            try
            {               
                SelectedPicture = this.Pictures.Where(x => x.PictureID == pictureId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "PickupPhoto");
            }
        }

        private async void PickupPhoto()
        {
            try
            {
                this.SelectedPicture = null;
                if (!_media.IsPickPhotoSupported)
                {
                    this.UserDialogs.Alert(StringResources.NoPickPhotoSupported);
                    return;
                }
                MediaFile CurrentImage;
                CurrentImage = await _media.PickPhotoAsync();
                if (CurrentImage == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "PickupPhoto");
            }
        }

        private async void TakePhoto()
        {
            try
            {
                if(this.MaximunPicturesAllowed.HasValue &&  this.Pictures?.Count()>this.MaximunPicturesAllowed.Value )
                {
                    return;
                }
                this.IsBusy = true;
                this.SelectedPicture = null;
                await ViewModelLocator.CheckCameraStoragePermission();                
                MediaFile CurrentImage;
                PictureDto picture = new PictureDto();

                if (!_media.IsCameraAvailable || !_media.IsTakePhotoSupported)
                {
                    this.UserDialogs.Alert(StringResources.NoAvailableCamera);
                    return;
                }

                CurrentImage = await _media.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = Services.Images.ImageManager.TEMP_IMAGE_FOLDER,
                    Name = Guid.NewGuid().ToString() + ".jpg",
                    AllowCropping = false
                });

                if (CurrentImage == null)
                {
                    return;

                }

                picture.ImageUrl = await ImageManager.SavePictureToDisk(CurrentImage.GetStream());
              //  picture.ImageArray= await ImageManager.GetPictureFromDisk(picture.ImageUrl);
                Pictures.Add(picture);
                this.SelectedPicture = picture;

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "TakePhoto");
            }
            finally
            {
               
                this.IsBusy = false;
            }
        }


    }
}
