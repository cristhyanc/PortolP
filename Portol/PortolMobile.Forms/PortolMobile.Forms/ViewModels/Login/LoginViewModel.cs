﻿using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {

      
        private readonly ILoginService _loginService;

        public ICommand LoginButtonCommand { get; private set; }
        public ICommand RecoverButtonCommand { get; private set; }
        public ICommand SignupCommand { get; private set; }

        private string _emailText;
        public string EmailText
        {
            get
            {
                return _emailText;
            }
            set
            {
                _emailText = value;
                OnPropertyChanged();
            }
        }

        private string _passwordText;
        public string PasswordText
        {
            get
            {
                return _passwordText;
            }
            set
            {
                _passwordText = value;
                OnPropertyChanged();
            }
        }


        public LoginViewModel(ILoginService loginService, INavigationService _navigationService, IUserDialogs _userDialogs) : base(_navigationService, _userDialogs)
        {
          
            _loginService = loginService;
            LoginButtonCommand = new Command(LoginUser);
            RecoverButtonCommand = new Command(GoToRecoverPassword);
            SignupCommand = new Command(GoToSignup);
            //this.EmailText = "cristhyan@msn.com";
            //this.PasswordText = "asd";
        }


        private async void GoToSignup()
        {
            try
            {
                   await NavigationService.NavigateToAsync<SignupStepMobileViewModel>();
                //var user = new Portol.Common.DTO.CustomerDto();
                //await NavigationService.NavigateToAsync<SignupStepAddressViewModel>(user);


            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToSignup");
            }
        }

        private async void GoToRecoverPassword()
        {
            try
            {
                await NavigationService.NavigateToAsync<RecoverPasswordViewModel>();
            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "GoToRecoverPassword");
            }

        }

        private async void LoginUser()
        {
            try
            {

                this.IsBusy = true;
                if (this.PasswordText?.Length > 0 && this.EmailText?.Length > 0)
                {
                    await SessionData.LoginUser(_loginService, this.EmailText, this.PasswordText);                    
                    await NavigationService.NavigateToAsync<DropViewModel>();
                }
                else
                {
                    UserDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.PasswordEmailRequired,
                        Title = StringResources.Login,
                        OkText = StringResources.Ok
                    });

                    return;
                }


            }
            catch (System.Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, StringResources.Login, "LoginUser");
            }
            finally
            {
                this.IsBusy = false;
            }
            //  
        }
    }
}
