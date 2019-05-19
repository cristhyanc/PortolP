using Naxam.Controls.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : TopTabbedPage
    {

        //  public NavigationPage NavPage { get { return this.nvgPage; } }

      //  public NavigationPage NavPage { get { return new NavigationPage(); } }
        public MainView()
        {
            InitializeComponent();
        
        }
              

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    INavigationService navigation = ViewModels.Base.ViewModelLocator.Resolve<INavigationService>();
        //    if (navigation != null)
        //    {
        //        navigation.SetNavigationPage(this.navigator);
        //    }

        //}

    }
}