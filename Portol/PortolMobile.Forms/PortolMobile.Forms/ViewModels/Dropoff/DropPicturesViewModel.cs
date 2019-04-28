using Acr.UserDialogs;
using Portol.Common.DTO;
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
  public  class DropPicturesViewModel: BaseViewModel
    {
        INavigationService navigationService;
        IUserDialogs userDialogs;
        public ICommand AddressEntryCommand { get; private set; }     

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

        public DropPicturesViewModel(INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
            navigationService = _navigationService;
            userDialogs = _userDialogs;          

         //   AddressEntryCommand = new Command<string>(((x) => GotoAddressPage(x)));

        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                var images = new List<PicturesDto>();
                images.Add(new PicturesDto { ImageUrl = "http://www.samoapost.ws/images/2017/05/14/parcel.jpg" });
                images.Add(new PicturesDto { ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cb/Parcelusarus2008.jpg/220px-Parcelusarus2008.jpg" });
                images.Add(new PicturesDto { ImageUrl = "https://s3-media3.fl.yelpcdn.com/bphoto/q3O8xKFjx1oDM_hZ1egJDQ/120s.jpg" });
                images.Add(new PicturesDto { ImageUrl = "https://previews.123rf.com/images/scanrail/scanrail1503/scanrail150300001/37439219-creative-abstract-shipping-logistics-and-retail-parcel-goods-delivery-commercial-business-concept-co.jpg" });
                ImagesTaken = images;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropAddressViewModel", "InitializeAsync");
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
