using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using Portol.Common.Interfaces.PortolMobile;
using PortolMobile.Core.ViewModels;
using PortolMobile.Core.ViewModels.Login;
using PortolMobile.Services.Rest;
using PortolMobile.Services.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolMobile.Core
{
  public  class App : MvxApplication
    {
        public override void Initialize()
        {
            try
            {
                //CreatableTypes()
                //    .EndingWith("Service")
                //    .AsInterfaces()
                //    .RegisterAsLazySingleton();

                //CreatableTypes()
                //    .EndingWith("Client")
                //    .AsInterfaces()
                //    .RegisterAsLazySingleton();
                Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
                Mvx.IoCProvider.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
                Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IRestClient, RestClient>();
                Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILoginService, LoginService>();
                
              
              //  var asd = Mvx.IoCProvider.Resolve<ILoginService>();
                //Mvx.IoCProvider.RegisterType<LoginViewModel>();
                // register the appstart object
                RegisterCustomAppStart<AppStart>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

          

          
        }
    }
}
