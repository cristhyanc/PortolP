using PortolMobile.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.Dropoff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DropIncomingDeliveryView : ExtendedContentPage
    {
        public DropIncomingDeliveryView()
        {
            InitializeComponent();
            this.IsTextBarWhite = true;
        }

        private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName.Equals("ItemsSource") && MyMap.ItemsSource!=null)
                {
                    List<Pin> pins = (List<Pin>)MyMap.ItemsSource;
                    if(pins?.FirstOrDefault()!=null)
                    {
                        MyMap.MoveToRegion(
                            MapSpan.FromCenterAndRadius(
                                pins.FirstOrDefault().Position , Distance.FromKilometers (4)));
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}