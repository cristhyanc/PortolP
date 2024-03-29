﻿using Acr.UserDialogs;
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
        
        public ICommand PickPicturesCommand { get; private set; }
        public ICommand MeasurementCommand { get; private set; }
        public ICommand GotoPaymentCommand { get; private set; }
        

        DeliveryDto _dropoffParcel;

        private List<PictureDto> _imagesTaken;
        public List<PictureDto> PicturesTaken
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

       
        decimal _worth;
        public decimal Worth
        {
            get
            {
                return _worth;
            }
            set
            {
                _worth = value;
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
                if (_dropoffParcel != null && _dropoffParcel.Parcel != null)
                {
                    return _dropoffParcel.Parcel.Volume.ToString() + " " + StringResources.M3;
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
           
            MeasurementCommand = new Command((() => GotoMeasurementPage()), () => { return !IsBusy; });
            PickPicturesCommand = new Command((() => GotoPicturesPickerPage()), () => { return !IsBusy; });
            GotoPaymentCommand = new Command((() => GotoPaymentPage()), () => { return !IsBusy; });
        }

        private async void GotoPaymentPage()
        {
            try
            {
                this.IsBusy = true;
                this._dropoffParcel.Pictures = this.PicturesTaken;
                if(_dropoffParcel.Parcel==null || _dropoffParcel.Parcel.Volume==0)
                {
                    UserDialogs.Alert(StringResources.MeasurementsRequired, StringResources.MissingInformation);
                    return;
                }

                if(_dropoffParcel.Pictures==null || _dropoffParcel.Pictures.Count==0)
                {
                    UserDialogs.Alert(StringResources.PictureParcelRequired, StringResources.MissingInformation);
                    return;
                }

                this._dropoffParcel.Parcel.Worth = Worth;

                await NavigationService.NavigateToAsync<DropPaymentViewModel>(this._dropoffParcel);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPicturesViewModel", "GotoPaymentPage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void GotoMeasurementPage()
        {
            try
            {
                this.IsBusy = true;
                SubscribeMeasurementMessagingService();
                await NavigationService.NavigateToAsync<DropMeasurementsViewModel>(this._dropoffParcel.Parcel);
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
                PicturePickerParameters picturePicker = new PicturePickerParameters();
                picturePicker.Pictures = this.PicturesTaken;
                picturePicker.MaximunPicturesAllowed = null;
                await NavigationService.NavigateToAsync<PicturePickerViewModel>(picturePicker);
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
                this._dropoffParcel = (DeliveryDto)navigationData;
                if (_dropoffParcel.Pictures?.Count > 0)
                {
                    this.PicturesTaken = _dropoffParcel.Pictures;
                }

                LoadGallery();

                //test
                _dropoffParcel.Parcel = new ParcelDto();
                _dropoffParcel.Parcel.Height = 2;
                _dropoffParcel.Parcel.Weight = 2;
                _dropoffParcel.Parcel.Length = 2;
                _dropoffParcel.Parcel.Width = 2;

                //test
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
               
                if (PicturesTaken?.Count > 0)
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
                MessagingCenter.Unsubscribe<PicturePickerViewModel, List<PictureDto>>(this, MessagingCenterCodes.PicturePickerMessage);
                MessagingCenter.Unsubscribe<DropMeasurementsViewModel, ParcelDto>(this, MessagingCenterCodes.MeasurementMessage);
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
                MessagingCenter.Subscribe<PicturePickerViewModel, List<PictureDto>>(this, MessagingCenterCodes.PicturePickerMessage, (sender, arg) =>
                {
                    if (arg?.Count > 0)
                    {
                        this.PicturesTaken = arg;
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
                MessagingCenter.Subscribe<DropMeasurementsViewModel, ParcelDto>(this, MessagingCenterCodes.MeasurementMessage, (sender, arg) =>
                {
                    if (arg != null)
                    {
                        this._dropoffParcel.Parcel = arg;
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
