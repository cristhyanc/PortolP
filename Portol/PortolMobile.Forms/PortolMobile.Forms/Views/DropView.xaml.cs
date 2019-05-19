using PortolMobile.Forms.Controls;
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
    public partial class DropView : ExtendedContentPage
    {
        public DropView()
        {
            InitializeComponent();
            ProfileImage.Focus();
        }
    }
}