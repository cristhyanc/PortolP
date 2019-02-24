using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.ViewModels;
using PortolMobile.Core.Resources;

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
               if(value)
                {
                    UserDialogs.ShowLoading();
                }
                else
                {
                    UserDialogs.HideLoading();
                }
                _isBusy = value;
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
        public string this[string index] => StringResc.ResourceManager.GetString(index);
    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        protected BaseViewModel()
        {
        }

        /// <summary>
        /// Gets the internationalized string at the given <paramref name="index"/>, which is the key of the resource.
        /// </summary>
        /// <param name="index">Index key of the string from the resources of internationalized strings.</param>
        public string this[string index] => StringResc.ResourceManager.GetString(index);
    }
}
