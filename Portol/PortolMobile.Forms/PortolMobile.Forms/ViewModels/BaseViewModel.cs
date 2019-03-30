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
      

        IUserDialogs _userDialogs;
        public  IUserDialogs UserDialogs
        {
            get
            {
                if(_userDialogs==null)
                {
                    _userDialogs = ViewModelLocator.Resolve<IUserDialogs>();
                }

                return _userDialogs;
            }
        }
        protected BaseViewModel()
        {
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
          
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }


    }

}
