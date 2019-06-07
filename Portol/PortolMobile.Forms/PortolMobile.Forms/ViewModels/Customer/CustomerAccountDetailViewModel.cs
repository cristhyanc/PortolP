using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Customer
{
  public  class CustomerAccountDetailViewModel: BaseViewModel
    {
        public ICommand SaveCommand { get; private set; }
        public ICommand GoToAddressCommand { get; private set; }
        public ICommand GoToPictureCommand { get; private set; }


        CustomerDto _user;
        public CustomerDto User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
              
            }
        }

        IUserCore _userCore;
        ISessionData _sessionData;
        public CustomerAccountDetailViewModel(INavigationService navigationService, IUserDialogs userDialogs, ISessionData sessionData, IUserCore userCore) : base(navigationService, userDialogs)
        {
            _userCore = userCore;
            _sessionData = sessionData;
            SaveCommand = new Command((() => SaveInformation()), () => { return !IsBusy; });
            GoToAddressCommand = new Command((() => GoToAddress()), () => { return !IsBusy; });
            GoToPictureCommand = new Command((() => GotoPicturesPickerPage()), () => { return !IsBusy; });
        }

        private async void GotoPicturesPickerPage()
        {
            try
            {
                this.IsBusy = true;

                PicturePickerParameters picturePicker = new PicturePickerParameters();              
                picturePicker.MaximunPicturesAllowed = 1;

                SubscribePicturesMessagingService();
                await NavigationService.NavigateToAsync<PicturePickerViewModel>(picturePicker);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "GotoPicturesPickerPage");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void GoToAddress()
        {
            try
            {
                this.IsBusy = true;
                AddressPickerParameters parameter = new AddressPickerParameters();
                parameter.Address = this.User.CustomerAddress;
              
                SubscribeMessagingService();
                await this.NavigationService.NavigateToAsync<AddressPickerViewModel>(parameter);
            }
            catch (Exception ex)
            {

                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "GoToAddress");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


        private async Task SaveInformation()
        {
            try
            {
                this.IsBusy = true;

                if (string.IsNullOrEmpty(User.FirstName) || string.IsNullOrEmpty(User.LastName) || string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.FirstName))
                {
                    this.UserDialogs.Alert(StringResources.AllFieldsRequired);
                    return;
                }

                User.ProfilePhoto.ImageArray = await Services.Images.ImageManager.GetPictureFromDisk(User.ProfilePhoto.ImageUrl);

                if (!await _userCore.SaveCustomer(User))
                {
                    this.UserDialogs.Alert(StringResources.ProblemTryAgain);
                    return;
                }

                await _sessionData.RefreshUserDetails(_userCore);
                await this.NavigationService.GoToPreviousPageAsync();

            }
            catch (Exception ex)
            {

                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "SaveInformation");
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
                User = _sessionData.User;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
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
                MessagingCenter.Unsubscribe<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "UnsubscribeMessagingService");
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
                     //   Save photo
                        this.User.ProfilePhoto  = arg.FirstOrDefault() ;
                        OnPropertyChanged("User");
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "SubscribePicturesMessagingService");
            }
        }

        private void SubscribeMessagingService()
        {
            try
            {
                MessagingCenter.Subscribe<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage, (sender, arg) =>
                {
                    if (arg != null && arg.Address != null)
                    {
                        this.User.CustomerAddress = arg.Address;
                        OnPropertyChanged("User");
                    }
                });

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "CustomerAccountDetailViewModel", "SubscribeMessagingService");
            }
        }
    }
}
