using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;

namespace WeatherDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ChooseLocationWindow : Window
    {
        public ChooseLocationWindow()
        {
            InitializeComponent();
            Tim_hub.Weather.API.key = "474d1400037bd7fae6326cf2389258bc";
        }

        private Location LastMouseClick;
        private bool MouseClicked = false;

        private void OnMyMapClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                LastMouseClick = myMap.ViewportPointToLocation(Mouse.GetPosition(myMap));
                MouseClicked = true;
                myMap_ViewChangeOnFrame(null, null);

                var mouseLocation = myMap.ViewportPointToLocation(new Point(MapMarker.Margin.Left, MapMarker.Margin.Top));
                MapPos.Text = Tim_hub.Weather.API.WeatherByCoordinates(new Tim_hub.Weather.API.Weather.Coord((float)mouseLocation.Latitude, (float)mouseLocation.Longitude)).CityName;
            }
        }
        private void myMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            if (MouseClicked)
            {
                var mouseScreenPosition = myMap.LocationToViewportPoint(LastMouseClick);//Mouse.GetPosition(myMap);
                MapMarker.Margin = new Thickness(mouseScreenPosition.X - MapMarker.Width / 2, mouseScreenPosition.Y, 0, 0);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myMap_ViewChangeOnFrame(null, null);
        }
    }
}
