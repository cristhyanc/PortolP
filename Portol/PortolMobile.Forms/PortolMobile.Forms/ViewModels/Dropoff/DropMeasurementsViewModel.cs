using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Helper;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
  public  class DropMeasurementsViewModel: BaseViewModel
    {

        public ICommand GoBackToPicturesCommand { get; private set; }

        

        MeasurementDto _measurements;
        public MeasurementDto Measurements {
            get
            {
                return _measurements;
            }
            set
            {
                _measurements = value;
                OnPropertyChanged();
            }

        }
        public DropMeasurementsViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            GoBackToPicturesCommand = new Command(GoToPicturePage);
        }

        private async void GoToPicturePage()
        {
            try
            {
                MessagingCenter.Send<DropMeasurementsViewModel, MeasurementDto>(this, MessagingCenterCodes.MeasurementMessage, this.Measurements);
                await this.NavigationService.GoToPreviousPageAsync();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropMeasurementsViewModel", "GoToPicturePage");                
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
               
             
                if (navigationData!=null)
                {
                    this.Measurements = (MeasurementDto)navigationData;
                }
                else
                {
                    this.Measurements = new MeasurementDto();
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropMeasurementsViewModel", "InitializeAsync");                
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
