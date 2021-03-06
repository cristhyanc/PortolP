﻿using Acr.UserDialogs;
using Autofac;
using Autofac.Core;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Portol.Calculator.Delivery;
using Portol.Calculator.Map;
using Portol.Calculator.Payment;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Core.DropOff;
using PortolMobile.Core.User;
using PortolMobile.Forms.Services;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels;
using PortolMobile.Forms.ViewModels.Customer;
using PortolMobile.Forms.ViewModels.Dropoff;
using PortolMobile.Forms.ViewModels.Login;
using PortolMobile.Forms.ViewModels.SignUp;
using PortolMobile.Forms.ViewModels.UserControls;
using PortolMobile.Services.Dropoff;
using PortolMobile.Services.Rest;
using PortolMobile.Services.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortolMobile.Forms
{
  public static class ViewModelLocator 
    {
        private static IContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }
      

        public static void RegisterDependencies(bool useMockServices)
        {
            try
            {


                var builder = new ContainerBuilder();

                builder.RegisterType<ShopViewModel>().SingleInstance();
                builder.RegisterType<DropViewModel>().SingleInstance();
                
                // View models
                builder.RegisterType<LoginViewModel>();
                builder.RegisterType<DropAddressViewModel>();
                builder.RegisterType<MainViewModel>();                
                builder.RegisterType<RecoverPasswordViewModel>();
                builder.RegisterType<SignupStepMobileViewModel>();
                builder.RegisterType<SignupStepEmailViewModel>();
                builder.RegisterType<SignupStepDetailsViewModel>();
                builder.RegisterType<SignupStepCodeViewModel>();
                builder.RegisterType<SignupStepAddressViewModel>();
                builder.RegisterType<AddressPickerViewModel>();
                builder.RegisterType<DropPicturesViewModel>();
                builder.RegisterType<PicturePickerViewModel>();
                builder.RegisterType<DropMeasurementsViewModel>();
                builder.RegisterType<DropPaymentViewModel>();
                builder.RegisterType<DropDriverInfoViewModel>();
                builder.RegisterType<CustomerPaymentMethodsViewModel>();
                builder.RegisterType<CustomerPaymentMethodViewModel>();
                builder.RegisterType<DropIncomingDeliveriesViewModel>();
                builder.RegisterType<CustomerAccountViewModel>();
                builder.RegisterType<CustomerAccountDetailViewModel>();
                builder.RegisterType<DropIncomingDeliveryViewModel>();
                

                builder.Register(c => UserDialogs.Instance).As<IUserDialogs>().SingleInstance();
                builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

                builder.RegisterType<DropoffMobileService>().As<IDeliveryMobileService>();
                builder.RegisterType<LoginService>().As<ILoginService>();

                builder.RegisterType<DeliveryCalculator>().As<IDeliveryCalculator>();
                builder.Register(c => new MapService("")).As<IMapService>();

                builder.RegisterType<DeliveryCore>().As<IDeliveryCore>();
                builder.Register(c => CrossMedia.Current).As<IMedia>();

                builder.RegisterType<LoginService>().As<ILoginService>();
                builder.RegisterType<UserMobileService>().As<IUserMobileService>();

                builder.RegisterType<UserCore>().As<IUserCore>();
                builder.RegisterType<LoginCore>().As<ILoginCore>();


                builder.Register(c => new PaymentService("sk_test_RCIYxJRaaXpKMgOIJnAvCrle00HTrJc29p", "pk_test_anYBo8LB4sisfeaXq8VvJOOJ00z6gDKo7R")).As<IPaymentService>();


                builder.RegisterType<SessionData>().As<ISessionData>().SingleInstance();
                builder.RegisterType<RestClient>().As<IRestClient>().WithParameter(new ResolvedParameter(
                                                                       (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "toke",
                                                                       (pi, ctx) => _container.Resolve<ISessionData>().GetCurrentToken()));

                builder.Register(c =>new AddressService(_container.Resolve<IRestClient>(), "", "")).As<IAddressService>().SingleInstance();

                if (_container != null)
                {
                    _container.Dispose();
                }
                _container = builder.Build();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static T Resolve<T>(params Autofac.Core.Parameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }

        public static T Resolve<T>(IEnumerable<Autofac.Core.Parameter> parameters)
        {
            return _container.Resolve<T>(parameters);
        }


        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            if (_container != null)
            {
                var viewModel = _container.Resolve(viewModelType);
                view.BindingContext = viewModel;
            }

        }

        //public static ILogError LogErrorService
        //{
        //    get { return ViewModelLocator.Resolve<ILogError>(); }

        //}

        public static async Task CheckCameraStoragePermission()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (cameraStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    cameraStatus = results[Permission.Camera];
                }

                if (storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                    storageStatus = results[Permission.Storage];
                }
            }  
             catch (Exception)
            {
                //TODO: Log
            }
        }

    }
}
