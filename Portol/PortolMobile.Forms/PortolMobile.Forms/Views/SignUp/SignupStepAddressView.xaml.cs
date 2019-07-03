using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupStepAddressView : ContentPage
    {
        public SignupStepAddressView()
        {
            InitializeComponent();
            
        }

        private void PgRight_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if(e.PropertyName.Equals("IsVisible") && pgRight.IsVisible )
                {
                    pgRight.ProgressTo(1, 2000, Easing.Linear);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}