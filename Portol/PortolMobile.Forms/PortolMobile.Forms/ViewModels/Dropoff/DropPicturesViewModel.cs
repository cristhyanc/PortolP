using Acr.UserDialogs;
using Portol.Common.DTO;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
  public  class DropPicturesViewModel: BaseViewModel
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
        public bool IsGalleryVisible {
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

        bool _isPicturePickerButtonVisible;
        public bool IsPicturePickerButtonVisible {
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
            MeasurementCommand = new Command((() => GotoMeasurementPage()));
            PickPicturesCommand = new Command((() => GotoPicturesPickerPage()));

        }

        private async void GotoMeasurementPage()
        {
            try
            {
               await NavigationService.NavigateToAsync<DropMeasurementsViewModel>();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "GotoMeasurementPage");
                this.IsBusy = false;
            }
        }

        private async void GotoPicturesPickerPage()
        {
            try
            {
                await NavigationService.NavigateToAsync<PicturePickerViewModel>();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "GotoPicturesPickerPage");
                this.IsBusy = false;
            }
        }

        public override  Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                this._dropoffParcel = (DropoffDto)navigationData;
                 LoadGallery();
                //var images = new List<PicturesDto>();
                //images.Add(new PicturesDto { ImageUrl = "http://www.samoapost.ws/images/2017/05/14/parcel.jpg" });
                //images.Add(new PicturesDto { ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cb/Parcelusarus2008.jpg/220px-Parcelusarus2008.jpg" });
                //images.Add(new PicturesDto { ImageUrl = "https://s3-media3.fl.yelpcdn.com/bphoto/q3O8xKFjx1oDM_hZ1egJDQ/120s.jpg" });
                //images.Add(new PicturesDto { ImageUrl = "https://previews.123rf.com/images/scanrail/scanrail1503/scanrail150300001/37439219-creative-abstract-shipping-logistics-and-retail-parcel-goods-delivery-commercial-business-concept-co.jpg" });
                //ImagesTaken = images;
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
                this.IsPicturePickerButtonVisible = false;
                this.IsGalleryVisible = false;
                if (_dropoffParcel.Images?.Count>0)
                {
                    this.IsGalleryVisible = true;
                }
                else
                {
                    this.IsPicturePickerButtonVisible = true;
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

    }
}
