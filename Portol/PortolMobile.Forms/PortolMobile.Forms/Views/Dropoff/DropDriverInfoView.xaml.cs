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
    public partial class DropDriverInfoView : ExtendedContentPage
    {
        public DropDriverInfoView()
        {
            InitializeComponent();
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
                            radius = Distance.FromKilometers(100 * (maxLongitude - minLongitude));
                        }
                        else
                        {
                            radius = Distance.FromKilometers(100 * (maxLatitude - minLatitude));
                        }

                        minLatitude = pins.Min(x => x.Position.Latitude);
                        var maxlLongitude = pins.Max(x => x.Position.Longitude);
                        var averageLongitude = pins.Average(x => x.Position.Longitude);
                        Position pin = new Position(minLatitude - 0.005, averageLongitude);

                        MyMap. MoveToRegion(
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