using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
   public class DropDriverInfoViewModel: BaseViewModel
    {
       

        bool _isSearchingDriver;
        public bool IsSearchingDriver
        {
            get
            {
                return _isSearchingDriver;
            }
            set
            {
                _isSearchingDriver = value;
                OnPropertyChanged();
            }

        }

        DriverDto _driverInfo;
        public DriverDto DriverInfo
        {
            get
            {
                return _driverInfo;
            }
            set
            {
                _driverInfo = value;
                OnPropertyChanged();
            }

        }

        DeliveryDto delivery;
        IDeliveryCore _deliveryCore;
        public DropDriverInfoViewModel(INavigationService navigationService, IUserDialogs userDialogs, IDeliveryCore deliveryCore) : base(navigationService, userDialogs)
        {
            _deliveryCore = deliveryCore;
        }


        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                delivery = (DeliveryDto)navigationData;
                IsSearchingDriver = true;
                GetDriverInformation(delivery.DeliveryID);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task GetDriverInformation(Guid deliveryID)
        {
            try
            {

                DateTime initialTime = DateTime.Now;

                while ((DateTime.Now - initialTime).Minutes < 1 && DriverInfo == null)
                {
                    await Task.Delay(5000);
                    DriverInfo = await _deliveryCore.GetDeliveryDriverInfo(deliveryID);
                }
                this.IsSearchingDriver = false;
            }
            catch (Exception ex)
            {
                this.IsSearchingDriver = false;
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "GetDriverInformation");
            }
        }
    }
}
