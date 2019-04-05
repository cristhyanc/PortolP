using PortolMobile.Forms.Effects;
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
    public partial class SignupStepDetailsView : ContentPage
    {
        public SignupStepDetailsView()
        {
            InitializeComponent();
            dtpBirth.Effects.Add(new BorderlessEffect());
        }
    }
}