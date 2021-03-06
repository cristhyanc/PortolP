﻿using Acr.UserDialogs;
using Portol.Common.DTO;
using Portol.Common.Helper;
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
    public class AddressPickerParameters
    {
       public  AddressDto Address { get; set; }
       public bool IsPickupAddress { get; set; }
    }
    public class AddressPickerViewModel: BaseViewModel
    {
        INavigationService _navigationService;
        IUserDialogs _userDialogs;
        IAddressService addressService;

        AddressPickerParameters parameter;

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
                if(value!=null)
                {
                    GetAddressDetails(value.id);
                }
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

        public AddressPickerViewModel(INavigationService navigationService, IUserDialogs userDialogs, IAddressService _addressService) : base(navigationService, userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            addressService = _addressService;
            TextChangedCommand = new Command<string>(PrepareSearchAddress, (x) => { return !IsBusy; });
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                _searchText = "";

                if (navigationData != null)
                {
                    parameter = (AddressPickerParameters)navigationData;
                    if (parameter != null && parameter.Address != null && !string.IsNullOrEmpty(parameter.Address.FullAddress))
                    {
                        this.SearchText = parameter.Address.FullAddress;
                        OnPropertyChanged("SearchText");
                        PrepareSearchAddress(parameter.Address.FullAddress);
                    }                    
                }
                else
                {
                    parameter = new AddressPickerParameters();
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
                if (text.Length > 5 )
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

        private async void GetAddressDetails(string addressFindeId)
        {
            try
            {
                this.IsBusy = true;
                if (!string.IsNullOrEmpty(addressFindeId))
                {
                    var result = await addressService.GetAddressMetadata(addressFindeId);
                    if (result != null)
                    {
                        parameter.Address = result.GetAddressDto();
                        MessagingCenter.Send<AddressPickerViewModel, AddressPickerParameters>(this, MessagingCenterCodes.AddressPickerMessage, parameter);
                    }
                    await _navigationService.GoToPreviousPageAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "PickupAddressViewModel", "GetAddressDetails");
            }
            finally
            {

            }
            
        }

    }
}
