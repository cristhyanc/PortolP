using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.ViewModels.Dropoff
{
    public class DropPaymentViewModel : BaseViewModel
    {
        IUserCore _userCore;
        public ICommand PaymentMethodListCommand { get; private set; }

        DropoffDto DropoffDetails { get; set; }

        private PaymentMethodDto _paymentMethodSelected;
        public PaymentMethodDto PaymentMethodSelected
        {
            get
            {
                return _paymentMethodSelected;
            }
            set
            {
                _paymentMethodSelected = value;
                OnPropertyChanged();
            }
        }

        private List<PaymentMethodDto> _paymentMethods;
        public List<PaymentMethodDto> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            set
            {
                _paymentMethods = value;
                OnPropertyChanged();
            }
        }


        public DropPaymentViewModel(IUserCore userCore, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            _userCore = userCore;
            PaymentMethodListCommand = new Command((() => OpenPaymentList()), () => { return !IsBusy; });
        }


        private void OpenPaymentList()
        {
            try
            {
                this.IsBusy = true;
                var cfg = new ActionSheetConfig()
                   .SetTitle(StringResources.Countries);
                foreach (var item in PaymentMethods)
                {
                    cfg.Add(
                       item.CardNumber,
                        () =>
                        {
                            this.PaymentMethodSelected  = item;
                        },item.IconName);
                    
                }

                cfg.SetCancel(null);
                var disp = this.UserDialogs.ActionSheet(cfg);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "OpenPaymentList");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                this.IsBusy = true;
                DropoffDetails = (DropoffDto)navigationData;
                PaymentMethods = _userCore.GetUserPaymentMethods();
                if(PaymentMethods?.Count>0)
                {
                    PaymentMethodSelected = PaymentMethods.Where(x => x.CurrentCard).FirstOrDefault();
                    if(PaymentMethodSelected==null)
                    {
                        PaymentMethodSelected = PaymentMethods.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "InitializeAsync");               
            }
            finally
            {
                this.IsBusy = false;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
