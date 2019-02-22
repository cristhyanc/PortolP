using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PortolMobile.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolMobile.Core
{
  public  class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
         
            // register the appstart object
            RegisterCustomAppStart<AppStart>();
        }
    }
}
