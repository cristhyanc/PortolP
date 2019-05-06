using PortolMobile.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.Dropoff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DropPicturesView : ExtendedContentPage
    {
        public DropPicturesView()
        {
            InitializeComponent();
        }

        private void ExtendedEntry_Focused(object sender, FocusEventArgs e)
        {
            var control = (ExtendedEntry)sender;
            control.Unfocus();
        }
    }
}