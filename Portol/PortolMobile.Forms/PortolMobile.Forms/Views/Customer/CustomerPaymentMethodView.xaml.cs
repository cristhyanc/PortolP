using PortolMobile.Forms.Controls;
using PortolMobile.Forms.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.Customer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerPaymentMethodView : ExtendedContentPage
    {
		public CustomerPaymentMethodView ()
		{
			InitializeComponent ();
            txtCreditNumber.Effects.Add(new BorderEffect());
            txtDate.Effects.Add(new BorderEffect());
            txtCvv.Effects.Add(new BorderEffect());
        }
	}
}