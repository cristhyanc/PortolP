using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortolMobile.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddressPickerView : ExtendedContentPage
    {
		public AddressPickerView()
		{
			InitializeComponent ();
            this.Appearing += AddressPickerView_Appearing;
		}

        private void AddressPickerView_Appearing(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }
    }
}