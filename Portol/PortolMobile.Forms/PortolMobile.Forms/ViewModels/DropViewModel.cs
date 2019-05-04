﻿using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Dropoff;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels
{
    public class DropViewModel : BaseViewModel
    {
        public ICommand GetCustomerCommand { get; private set; }
        public ICommand GotoShopCommand { get; private set; }
        private ICustomerMobileService _customerService;

        string _emailMobileNumber;
        public string EmailMobileNumber
        {
            get { return _emailMobileNumber; }
            set
            {
                _emailMobileNumber = value;
                OnPropertyChanged();
            }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        string _receiverName;
        public string ReceiverName
        {
            get { return _receiverName; }
            set
            {
                _receiverName = value;
                OnPropertyChanged();
            }
        }

      

        public DropViewModel(ICustomerMobileService customerService, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            GetCustomerCommand = new Command((() => GetCustomer()));
            GotoShopCommand = new Command(GotoShop);
            _customerService = customerService;
            this.EmailMobileNumber = "0405593358";
            this.ReceiverName = "Cris";
            this.Description = "ddd";

           
        }

        //public override async Task InitializeAsync(object navigationData)
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.ProcessException(ex, UserDialogs, "DropViewModel", "InitializeAsync");
              
        //    }
           
        //}

        private async void GotoShop()
        {
            await NavigationService.NavigateToAsync<ShopViewModel>();
        }

        public async Task GetCustomer()
        {
            try
            {
                if (this.IsBusy)
                {
                    return;
                }

                if (!FieldsValidation())
                {
                    return;
                }

                this.IsBusy = true;
                CustomerDto customer = null;
                long number = 0;

                if (long.TryParse(this.EmailMobileNumber, out number))
                {
                    customer = await _customerService.GetCustomerByPhoneNumber(number, SessionData.User.PhoneCountryCode);
                    
                }              
                               

                if (customer == null)
                {
                    if (Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                    {
                        customer = await _customerService.GetCustomerByEmail(EmailMobileNumber);
                    }
                    
                    if (customer == null)
                    {
                        if (!await this.DisplayMessageQuestion(StringResources.Guess, StringResources.PersonNoRegistered, StringResources.ContinueGuess))
                        {
                            return;
                        }

                        customer = new CustomerDto();
                        customer.PhoneNumber = number;

                        customer.Email = StringResources.GuessEmail;
                        if (Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                        {
                            customer.Email = this.EmailMobileNumber;
                        }
                                                    
                        customer.PhoneCountryCode = SessionData.User.PhoneCountryCode;
                        customer.IsGuess = true;
                        customer.FirstName = this.ReceiverName;
                    }
                }

                DropoffDto dropoffDto = new DropoffDto();
                dropoffDto.Receiver = customer;
                dropoffDto.Description = this.Description;
                dropoffDto.Sender = SessionData.User;


                await NavigationService.NavigateToAsync<DropAddressViewModel>(dropoffDto);
              

            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.MainPage, "GetCustomer");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private bool FieldsValidation()
        {
            if (string.IsNullOrEmpty(this.EmailMobileNumber))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberEmailRequiered);
                return false;
            }

            long number = 0;
            if (!long.TryParse(this.EmailMobileNumber, out number))
            {
               

                if (!Regex.IsMatch(EmailMobileNumber, Portol.Common.Helper.Constants.RegexEmailPattern))
                {
                    this.DisplayMessage(StringResources.MissingInformation, StringResources.MobileNumberEmailRequiered);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(this.ReceiverName))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.ReceiverNameRequired);
                return false;
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                this.DisplayMessage(StringResources.MissingInformation, StringResources.DescriptionRequired);
                return false;
            }

            return true;
        }
    }
}
