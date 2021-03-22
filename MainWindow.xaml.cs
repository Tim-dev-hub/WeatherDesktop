using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private Location LastMouseClick;
        private bool MouseClicked = false;

        private void OnMyMapClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                MapMarker.Visibility = Visibility.Visible;
                LastMouseClick = myMap.ViewportPointToLocation(Mouse.GetPosition(myMap));
                MouseClicked = true;
                myMap_ViewChangeOnFrame(null, null);

                var mouseLocation = myMap.ViewportPointToLocation(new Point(MapMarker.Margin.Left, MapMarker.Margin.Top));
                User.Location = new Tim_hub.Weather.API.Weather.Coord((float)mouseLocation.Latitude, (float)mouseLocation.Longitude);
                Weather.UpdateWeather();
                var sms = Weather.Current;
                MapPos.Text = $"Location : {Weather.Current.CityName}" ;
            }
        }

        private void myMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            if (MouseClicked)
            {
                var mouseScreenPosition = myMap.LocationToViewportPoint(LastMouseClick);
                MapMarker.Margin = new Thickness(mouseScreenPosition.X - MapMarker.Width / 2, mouseScreenPosition.Y, 0, 0);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myMap_ViewChangeOnFrame(null, null);
        }

        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            if(User.Location != null)
            {
                MapMarker.Visibility = Visibility.Visible;
                LastMouseClick = new Location(User.Location.Latitude, User.Location.Longitude);
                MouseClicked = true;
                var mouseScreenPosition = myMap.LocationToViewportPoint(LastMouseClick);
                MapMarker.Margin = new Thickness(mouseScreenPosition.X - MapMarker.Width / 2, mouseScreenPosition.Y, 0, 0);
                MapPos.Text = $"Location : {Weather.Current.CityName}";
            }
        }

        private void WeatherLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWeather(Weather.Current);
            Weather.OnWeatherUpdate += UpdateWeather;
        }

        private void UpdateWeather(Tim_hub.Weather.API.Weather arg)
        {
            if (User.Location != null)
            {
                Console.WriteLine("UPDATE");
                string exePath = this.GetType().Assembly.Location;
                var locationPaths = exePath.Split('\\').ToList();
                string programLocation = exePath.Replace(locationPaths[locationPaths.Count - 1], "");
                var pack = new WeatherDesktop.Packs.WeatherPack(programLocation + @"\Assets\WeatherPacks\WeatherDefault");

                var iconID = Weather.Current.Condition[0].IconID;
                SetWeatherFontColor(iconID[iconID.Length - 1] == 'd' ? Brushes.Black : Brushes.White);

                string iconPath = ProgramCache.FilesPath + iconID + ".png";
                if (!System.IO.File.Exists(iconPath))
                {

                    var icon = Tim_hub.Weather.API.DownloadIcon(iconID);
                    icon.Save(iconPath);
                }

                UpdateImageSource(WeatherIcon, iconPath);


                WeatherHumidity.Text = Weather.Current.Measurements.Humidity + " %";
                WeatherPressure.Text = Weather.Current.Measurements.Pressure + " hPa";
                WeatherWind.Text = Weather.Current.Wind.Speed + " m/s";
                var temper = Weather.Current.Measurements.Temperatur;
                WeatherTemperature.Text = temper > 0 ? $"+{temper}" : temper.ToString();
                WeatherTemperature.Text += "°C";
                WeatherDeskription.Text = Weather.Current.Condition[0].Description;
                CityNameWeather.Text = Weather.Current.CityName;

                var weatherBack = pack.GetWallpaper(Weather.Current);
                UpdateImageSource(WeatherBack, weatherBack);
                DisplayPicture(weatherBack.Replace(@"\\", @"\").Replace(@"\\\", @"\".Replace(@"\\\\", @"\")));
                //just don't ask...
            }
            else
            {
                MainTabContol.SelectedIndex = 2;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);

        private const uint SPI_SETDESKWALLPAPER = 0x14;
        private const uint SPIF_UPDATEINIFILE = 0x1;
        private const uint SPIF_SENDWININICHANGE = 0x2;

        private static void DisplayPicture(string file_name)
        {
            uint flags = 0;
            if (!SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0, file_name, flags))
            {
                Console.WriteLine("Error");
            }
        }

    private void SetWeatherFontColor(SolidColorBrush brush)
        {
            WeatherHumidity.Foreground = brush;
            WeatherPressure.Foreground = brush;
            WeatherWind.Foreground = brush;
            WeatherTemperature.Foreground = brush;
            WeatherDeskription.Foreground = brush;
            CityNameWeather.Foreground = brush;
        }

        private void UpdateImageSource(Image image, string source)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();

            bi3.UriSource = new Uri(source);
            bi3.EndInit();

            image.Source = bi3;
        }
        private void WeatherTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (User.Location == null)
            {
                MainTabContol.SelectedIndex = 2;
            }
        }
    }
}
