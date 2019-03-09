using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.ViewModels;
using Portol.Common;

namespace PortolMobile.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        private bool  _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {              
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        IUserDialogs _userDialogs;
        public  IUserDialogs UserDialogs
        {
            get
            {
                if(_userDialogs==null)
                {
                    _userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
                }

                return _userDialogs;
            }
        }
        protected BaseViewModel()
        {
           
        }

        /// <summary>
        /// Gets the internationalized string at the given <paramref name="index"/>, which is the key of the resource.
        /// </summary>
        /// <param name="index">Index key of the string from the resources of internationalized strings.</param>
        public string this[string index] => StringResources.ResourceManager.GetString(index);
    }

    public abstract class BaseViewModel<TParameter> : MvxViewModel<TParameter>
        where TParameter : class
    {
        protected BaseViewModel()
        {
        }

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
                RaisePropertyChanged(() => IsBusy);
            }
        }

        IUserDialogs _userDialogs;
        public IUserDialogs UserDialogs
        {
            get
            {
                if (_userDialogs == null)
                {
                    _userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
                }

                return _userDialogs;
            }
        }

        /// <summary>
        /// Gets the internationalized string at the given <paramref name="index"/>, which is the key of the resource.
        /// </summary>
        /// <param name="index">Index key of the string from the resources of internationalized strings.</param>
        public string this[string index] => StringResources.ResourceManager.GetString(index);
    }
}
