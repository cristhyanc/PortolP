using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropPicturesViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IUserDialogs _userDialogs;
        public ICommand PickPicturesCommand { get; private set; }
        public ICommand MeasurementCommand { get; private set; }

        DropoffDto _dropoffParcel;

        private List<PicturesDto> _imagesTaken;
        public List<PicturesDto> ImagesTaken
        {
            get
            {
                return _imagesTaken;
            }
            set
            {
                _imagesTaken = value;
                OnPropertyChanged();
            }
        }

        bool _isGalleryVisible;
        public bool IsGalleryVisible
        {
            get
            {
                return _isGalleryVisible;
            }
            set
            {
                _isGalleryVisible = value;
                OnPropertyChanged();
            }
        }

        public string ParcelVolume
        {
            get
            {
                if (_dropoffParcel != null && _dropoffParcel.Measurements != null)
                {
                    return _dropoffParcel.Measurements.Volume.ToString() + " " + StringResources.M3;
                }
                return null;
            }

        }

        bool _isPicturePickerButtonVisible;
        public bool IsPicturePickerButtonVisible
        {
            get
            {
                return _isPicturePickerButtonVisible;
            }
            set
            {
                _isPicturePickerButtonVisible = value;
                OnPropertyChanged();
            }
        }


        public DropPicturesViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            MeasurementCommand = new Command((() => GotoMeasurementPage()), () => { return !IsBusy; });
            PickPicturesCommand = new Command((() => GotoPicturesPickerPage()), () => { return !IsBusy; });

        }

        private async void GotoMeasurementPage()
        {
            try
            {
                this.IsBusy = true;
                SubscribeMeasurementMessagingService();
                await NavigationService.NavigateToAsync<DropMeasurementsViewModel>(this._dropoffParcel.Measurements);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "GotoMeasurementPage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void GotoPicturesPickerPage()
        {
            try
            {
                this.IsBusy = true;
                SubscribePicturesMessagingService();
                await NavigationService.NavigateToAsync<PicturePickerViewModel>(this.ImagesTaken);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "GotoPicturesPickerPage");
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
                this._dropoffParcel = (DropoffDto)navigationData;
                if (_dropoffParcel.Images?.Count > 0)
                {
                    this.ImagesTaken = _dropoffParcel.Images;
                }

                LoadGallery();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "InitializeAsync");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }

        private void LoadGallery()
        {
            try
            {              
               
                if (ImagesTaken?.Count > 0)
                {
                    this.IsGalleryVisible = true;
                    this.IsPicturePickerButtonVisible = false;
                }
                else
                {
                    this.IsPicturePickerButtonVisible = true;
                    this.IsGalleryVisible = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "LoadGallery");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        protected override void PageAppearing()
        {
            UnsubscribeMessagingService();
        }

        private void UnsubscribeMessagingService()
        {
            try
            {
                MessagingCenter.Unsubscribe<PicturePickerViewModel, List<PicturesDto>>(this, MessagingCenterCodes.PicturePickerMessage);
                MessagingCenter.Unsubscribe<DropMeasurementsViewModel, MeasurementDto>(this, MessagingCenterCodes.MeasurementMessage);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "UnsubscribeMessagingService");
            }
        }

        private void SubscribePicturesMessagingService()
        {
            try
            {
                MessagingCenter.Subscribe<PicturePickerViewModel, List<PicturesDto>>(this, MessagingCenterCodes.PicturePickerMessage, (sender, arg) =>
                {
                    if (arg?.Count > 0)
                    {
                        this.ImagesTaken = arg;
                        LoadGallery();
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "SubscribePicturesMessagingService");
            }
        }

        private void SubscribeMeasurementMessagingService()
        {
            try
            {
                MessagingCenter.Subscribe<DropMeasurementsViewModel, MeasurementDto>(this, MessagingCenterCodes.MeasurementMessage, (sender, arg) =>
                {
                    if (arg != null)
                    {
                        this._dropoffParcel.Measurements = arg;
                        OnPropertyChanged("ParcelVolume");
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "SubscribeMeasurementMessagingService");
            }
        }

    }
}
