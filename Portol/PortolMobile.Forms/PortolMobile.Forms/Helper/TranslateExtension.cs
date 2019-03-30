using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Helper
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        string ResourceId = "PortolMobile.Forms.Resources.AppResources";

        private static Lazy<ResourceManager> resMgr = null;

        //public TranslateExtension()
        //{
        //   // ci = new CultureInfo("en");
        //    if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
        //    {
        //       // ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        //    }
        //}

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            if (resMgr == null)
            {
                var assembly = GetType().GetTypeInfo().Assembly;
                string assemblyName = assembly.GetName().Name;
               // ResourceId = assemblyName + ".Resources.AppResources";
                resMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, assembly));

            }

            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = resMgr.Value.GetString(Text, ci);

            if (translation == null)
            {
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
            }
            return translation;

        }
    }
}