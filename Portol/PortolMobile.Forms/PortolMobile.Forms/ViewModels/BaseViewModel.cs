using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Portol.Common;
using PortolMobile.Forms.Services.Navigation;

namespace PortolMobile.Forms.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
      

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                              _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected readonly INavigationService NavigationService;
        protected readonly IUserDialogs UserDialogs;

        //IUserDialogs _userDialogs;
        //public  IUserDialogs UserDialogs
        //{
        //    get
        //    {
        //        if(_userDialogs==null)
        //        {
        //            _userDialogs = ViewModelLocator.Resolve<IUserDialogs>();
        //        }

        //        return _userDialogs;
        //    }
        //}



        protected BaseViewModel(INavigationService _navigationService, IUserDialogs _userDialogs)
        {
           // NavigationService = ViewModelLocator.Resolve<INavigationService>();
            NavigationService = _navigationService;
            UserDialogs = _userDialogs;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public void DisplayMessage(string title, string message, string okButton="Ok")
        {
            UserDialogs.Alert(new AlertConfig
            {
                Message = message,
                Title = title,
                OkText = okButton
            });
        }

        public Task<bool> DisplayMessageQuestion(string title, string message, string okButton = "Ok", string cancelButton = "Cancel")
        {
            return UserDialogs.ConfirmAsync(new ConfirmConfig
            {
                CancelText = cancelButton,
                Message= message,
                OkText= okButton,
                Title= title
            });
        }

    }

}
