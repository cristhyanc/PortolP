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
    public partial class SignupStepCodeView : ContentPage
    {
        public SignupStepCodeView()
        {
            InitializeComponent();
        }

        private void TxtN1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ttxn = (Entry)sender;
            if (!string.IsNullOrEmpty(ttxn.Text) && ttxn.Text.Length > 0)
            {
                if (ttxn == txtN1)
                {
                    txtN2.Focus();
                }

                if (ttxn == txtN2)
                {
                    txtN3.Focus();
                }

                if (ttxn == txtN3)
                {
                    txtN4.Focus();
                }
                             
            }

        }
    }
}