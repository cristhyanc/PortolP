using Acr.UserDialogs;
using Portol.Common.DTO;
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

        DeliveryDto delivery;

        public DropDriverInfoViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            
        }


        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                delivery = (DeliveryDto)navigationData;
                IsSearchingDriver = true;

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropDriverInfoViewModel", "InitializeAsync");
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
