using Acr.UserDialogs;
using Autofac;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Forms.Services.Navigation;
using PortolMobile.Forms.ViewModels;
using PortolMobile.Forms.ViewModels.Login;
using PortolMobile.Services.Rest;
using PortolMobile.Services.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
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

                // View models
                builder.RegisterType<LoginViewModel>();              
                builder.RegisterType<MainViewModel>();
                builder.RegisterType<RecoverPasswordViewModel>();
                

                builder.Register(c => UserDialogs.Instance).As<IUserDialogs>().SingleInstance();
                builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();


                builder.RegisterType<RestClient>().As<IRestClient>();
                builder.RegisterType<LoginService>().As<ILoginService>();
                builder.RegisterType<UserMobileService>().As<IUserMobileService>();

              

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

    }
}