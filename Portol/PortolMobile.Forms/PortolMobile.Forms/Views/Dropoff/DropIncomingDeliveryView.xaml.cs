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
                if (e.PropertyName.Equals("ItemsSource") && MyMap.ItemsSource != null)
                {
                    List<Pin> pins = (List<Pin>)MyMap.ItemsSource;
                    if (pins?.FirstOrDefault() != null)
                    {
                        var minLatitude = pins.Min(x => Math.Abs(x.Position.Latitude));
                        var maxLatitude = pins.Max(x => Math.Abs(x.Position.Latitude));

                        var minLongitude = pins.Min(x => Math.Abs(x.Position.Longitude));
                        var maxLongitude = pins.Max(x => Math.Abs(x.Position.Longitude));

                        Distance radius = Distance.FromKilometers(10);
                        if (maxLongitude - minLongitude > maxLatitude - minLatitude)
                        {
                            radius = Distance.FromKilometers(90 * (maxLongitude - minLongitude));
                        }
                        else
                        {
                            radius = Distance.FromKilometers(90 * (maxLatitude - minLatitude));
                        }
                                             
                        var averageLatitude = pins.Average(x => x.Position.Latitude );
                        var averageLongitude = pins.Average(x => x.Position.Longitude);
                        Position pin = new Position(averageLatitude, averageLongitude);

                        MyMap.MoveToRegion(
                            MapSpan.FromCenterAndRadius(
                                pin, radius));

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}