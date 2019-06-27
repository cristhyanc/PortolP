using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Helper;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels.Customer;
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
        public ICommand ConfirmCommand { get; private set; }
        public ICommand AddPaymentMethodCommand { get; private set; }

        
        DeliveryDto DropoffDetails { get; set; }

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

        decimal _estimatedMinCost;
        public decimal EstimatedMinCost
        {
            get
            {
                return _estimatedMinCost;
            }
            set
            {
                _estimatedMinCost = value;
                OnPropertyChanged();
            }
        }

        decimal _estimatedMaxCost;
        public decimal EstimatedMaxCost
        {
            get
            {
                return _estimatedMaxCost;
            }
            set
            {
                _estimatedMaxCost = value;
                OnPropertyChanged();
            }
        }



        IDeliveryCalculator _dropoffCalculatorService;
        IDeliveryCore _deliveryCore;
        ISessionData _sessionData;
        IPaymentService _paymentService;

        public DropPaymentViewModel(IUserCore userCore, INavigationService navigationService, IUserDialogs userDialogs, IDeliveryCalculator dropoffCalculator, IDeliveryCore dropoffCore, ISessionData sessionData, IPaymentService paymentService) : base(navigationService, userDialogs)
        {
            _userCore = userCore;
            PaymentMethodListCommand = new Command((() => OpenPaymentList()), () => { return !IsBusy; });
            ConfirmCommand = new Command((() => ConfirmService()), () => { return !IsBusy; });
            AddPaymentMethodCommand = new Command((() => GoToPaymentMethods()), () => { return !IsBusy; });
            _dropoffCalculatorService = dropoffCalculator;
            _deliveryCore = dropoffCore;
            _sessionData = sessionData;
            _paymentService = paymentService;
           
        }

        private async void GoToPaymentMethods()
        {
            try
            {               
                this.IsBusy = true;                
                await this.NavigationService.NavigateToAsync<CustomerPaymentMethodsViewModel>();                    

            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "GoToPaymentMethods");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


        private async void ConfirmService()
        {
            try
            {
                this.IsBusy = true;

                if (this.PaymentMethodSelected == null && (this.PaymentMethods == null || this.PaymentMethods.Count == 0))
                {
                    await this.NavigationService.NavigateToAsync<CustomerPaymentMethodsViewModel>(this.DropoffDetails.Sender);
                    return;
                }

                if (this.EstimatedMinCost == 0)
                {
                    LoadInformation();
                  
                    return;
                }

                this.DropoffDetails.EstimatedCost = this.EstimatedMinCost;
                this.DropoffDetails.PaymentMethod = this.PaymentMethodSelected;

                if (this.DropoffDetails.Pictures?.Count > 0)
                {
                    foreach (var item in this.DropoffDetails.Pictures)
                    {
                        item.ImageArray = await Services.Images.ImageManager.GetPictureFromDisk(item.ImageUrl);
                    }
                }
                this.DropoffDetails.CreatedDate = DateTime.Now;
                var result = await _deliveryCore.CreateDropoffRequest(this.DropoffDetails);
                if (result != Guid.Empty )
                {
                    this.DropoffDetails.DeliveryID = result;                  
                    await this.NavigationService.NavigateToAsync<DropDriverInfoViewModel>(this.DropoffDetails);
                }
                else
                {
                    this.UserDialogs.Alert(StringResources.ProblemTryAgain);
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
        }

        private void OpenPaymentList()
        {
            try
            {
                this.IsBusy = true;
                var cfg = new ActionSheetConfig()
                   .SetTitle(StringResources.PaymentMethod);
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


        protected override  async void PageAppearing()
        {
            try
            {
                var delivery = await _deliveryCore.GetSendertDeliveryInProgress(_sessionData.User.CustomerID);
                if (delivery != null)
                {
                    await NavigationService.NavigateToAsync<DropDriverInfoViewModel>(delivery);
                }
                else
                {
                    await LoadInformation();
                }
                
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                this.UserDialogs.HideLoading();
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "PageAppearing");
            }
        }

        public async Task LoadInformation()
        {
            try
            {

                if(DropoffDetails==null)
                {
                    return;
                }
                List<PaymentMethodDto> PaymentMethods = null;

                UserDialogs.ShowLoading("Calculating...");
                var task2 = _paymentService.GetCustomerPaymentMethods(_sessionData.User.CustomerPaymentID);
                var tasks = new List<Task> { EstimatePrice(), task2 };
                await Task.WhenAll(tasks);

                PaymentMethods = task2.Result;
                if (PaymentMethods?.Count > 0)
                {
                    PaymentMethodSelected = PaymentMethods.Where(x => x.CurrentCard).FirstOrDefault();
                    if (PaymentMethodSelected == null)
                    {
                        PaymentMethodSelected = PaymentMethods.FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {              
                this.UserDialogs.HideLoading();
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "LoadInformation");
            }
            finally
            {
                this.IsBusy = false;
                this.UserDialogs.HideLoading();
            }

        }
        
        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                DropoffDetails = (DeliveryDto)navigationData;
                await LoadInformation();
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                this.UserDialogs.HideLoading();
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "InitializeAsync");
            }
            await base.InitializeAsync(navigationData);
        }

        private async Task EstimatePrice()
        {
            try
            {               
                List<VehiculeTypeDto> vehiculeTypesAvailable = await _deliveryCore.GetVehiculeTypesAvailables();
                var costs = await _dropoffCalculatorService.EstimatePrice(DropoffDetails.Parcel, DropoffDetails.PickupAddress, DropoffDetails.DropoffAddress, vehiculeTypesAvailable);
                EstimatedMinCost = costs.Min();
                EstimatedMaxCost = costs.Max();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, UserDialogs, "DropPaymentViewModel", "EstimatePrice");
            }
           
        }
    }
}
