using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using PortolMobile.Forms.Helper;
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
   public class PicturePickerViewModel: BaseViewModel
    {

        
        public ICommand TakePhotoCommand { get; private set; }
        public ICommand PickupVideoCommand { get; private set; }
        public ICommand TakeVideoCommand { get; private set; }
        public ICommand PickupPhotoCommand { get; private set; }
        public ICommand SelectedPhotoCommand { get; private set; }
        public ICommand DoneCommand { get; private set; }

        
        PicturesDto _selectedPicture;     
        public PicturesDto SelectedPicture
        {
            get
            {
                return _selectedPicture;
            }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged();
            }

        }

        ObservableCollection <PicturesDto> _pictures;
        public ObservableCollection<PicturesDto> Pictures {
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

        IMedia _media;
        public PicturePickerViewModel(IMedia media, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            _media = media;
            TakePhotoCommand = new Command(TakePhoto, () => { return !IsBusy; });
            PickupPhotoCommand = new Command(PickupPhoto, () => { return !IsBusy; });
            SelectedPhotoCommand = new Command<Guid>(PhotoSelected, (x) => { return !IsBusy; });
            DoneCommand = new Command(GoBack, () => { return !IsBusy; });
            Pictures = new ObservableCollection<PicturesDto>();
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                if(navigationData!=null)
                {
                    IEnumerable <PicturesDto> picturesDtos = (IEnumerable<PicturesDto>)navigationData;
                    Pictures = new ObservableCollection<PicturesDto>(picturesDtos);
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
                MessagingCenter.Send<PicturePickerViewModel, List<PicturesDto>>(this, MessagingCenterCodes.PicturePickerMessage, this.Pictures.ToList());
                await this.NavigationService.GoToPreviousPageAsync();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PicturePickerViewModel", "GoBack");
            }
        }

        private  void PhotoSelected(Guid pictureId)
        {
            try
            {
                //var id = Guid.Parse(pictureId);
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
                if(!_media.IsPickPhotoSupported)
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
              await   ViewModelLocator.CheckCameraStoragePermission();
                this.IsBusy = true;
                MediaFile CurrentImage;
                PicturesDto pictures = new PicturesDto();

                if (!_media.IsCameraAvailable || !_media.IsTakePhotoSupported)
                {
                    this.UserDialogs.Alert(StringResources.NoAvailableCamera);
                    return;
                }
                                
                CurrentImage = await _media.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "tempPhotos",
                    Name = Guid.NewGuid().ToString() + ".jpg",
                    AllowCropping = false
                });
                if (CurrentImage == null)
                {
                    return;

                }

                pictures.Image = ImageSource.FromStream(() =>
                {
                    return CurrentImage.GetStream();
                });


                Pictures.Add(pictures);              


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
