using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.UserControls
{
    public class AddressPickerViewModel: BaseViewModel
    {
        INavigationService navigationService;
        IUserDialogs userDialogs;
        IAddressService addressService;

        AddressDto pickedAddress;

        string _searchText;
        public string SearchText
        {
            get
            {
                if (string.IsNullOrEmpty(_searchText))
                {
                    _searchText = "";
                }
                return _searchText;
            }
            set
            {
                _searchText = value;
            }
        }

        AddressFinderResultDto _selectedAddress;
        public AddressFinderResultDto SelectedAddress
        {
            get
            {
                return _selectedAddress;
            }
            set
            {
                _selectedAddress = value;
                OnPropertyChanged();
            }
        }

        List < AddressFinderResultDto>  _possibleAddress;
        public List<AddressFinderResultDto> PossibleAddress
        {
            get
            {               
                return _possibleAddress;
            }
            set
            {
                _possibleAddress = value;
                OnPropertyChanged();
            }
        }
        public ICommand TextChangedCommand { get; private set; }
        private static CancellationTokenSource globalSuggestionsCts;

        public AddressPickerViewModel(INavigationService _navigationService, IUserDialogs _userDialogs, IAddressService _addressService) : base(_navigationService, _userDialogs)
        {
            navigationService = _navigationService;
            userDialogs = _userDialogs;
            addressService = _addressService;
            TextChangedCommand = new Command<string>(PrepareSearchAddress);
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                _searchText = "";
                pickedAddress = new AddressDto();
                if (navigationData != null)
                {
                    pickedAddress = (AddressDto)navigationData;
                    this.SearchText = pickedAddress.FullAddress;
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PickupAddressViewModel", "InitializeAsync");

            }
            return base.InitializeAsync(navigationData);
        }

        private void PrepareSearchAddress(string text)
        {
            try
            {
                if (text.Length > 5 && text.Length % 2 == 0)
                {
                    globalSuggestionsCts?.Cancel();
                    this.IsBusy = true;
                    Task.Run(() =>
                    {
                        globalSuggestionsCts = new CancellationTokenSource();
                        SearchAddress(text,globalSuggestionsCts);
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PickupAddressViewModel", "PrepareSearchAddress");
            }
        }

       

        private async void SearchAddress(string text, CancellationTokenSource suggestionsCts)
        {
            try
            {
                if (suggestionsCts.Token.IsCancellationRequested) return;
                var listResult = await addressService.GetPosibleAddresses(text);
                if (suggestionsCts.Token.IsCancellationRequested) return;

                if (listResult.completions?.Count>0 )
                {
                    PossibleAddress = listResult.completions;
                }
                else
                {
                    PossibleAddress = new List<AddressFinderResultDto>();
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "AddressPickerViewModel", "SearchAddress");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


    }
}
