using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Json;
using PortolMobile.Core;
using PortolMobile.iOS.MvxBindings;
using UIKit;

namespace PortolMobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var registry = Mvx.Resolve<IMvxTargetBindingFactoryRegistry>();
            registry.RegisterFactory(new MvxCustomBindingFactory<UIViewController>("NetworkIndicator", (viewController) => new NetworkIndicatorTargetBinding(viewController)));
        }

        protected override IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions
            {
                PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
            };
        }
    }
}
