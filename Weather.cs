using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim_hub.Weather;

namespace WeatherDesktop
{
    public static class Weather
    {
        public static API.Weather Current { get; private set; }
        public delegate void WeatherUpdate(API.Weather arg);
        public static event WeatherUpdate OnWeatherUpdate;


        private static System.Timers.Timer _weatherUpdater;
         static Weather() 
        {
            if(Current == null)
            {
                API.key = "474d1400037bd7fae6326cf2389258bc";
                UpdateWeather();

                //update weather every 30 minutes
                _weatherUpdater = new System.Timers.Timer(1000 * 60 * 30);
                _weatherUpdater.Elapsed += WeatherUpdater_Elapsed;
                _weatherUpdater.Start();
            }
        }

        public static API.Weather UpdateWeather()
        {
            if (User.Location != null)
            {
                Current = API.WeatherByCoordinates(User.Location, API.WetherUnits.metric, "ru");
                CallWeatherUpdate();
                return Current;
            }
            return null;
        }

        private static void WeatherUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CallWeatherUpdate()
        {
            if(OnWeatherUpdate != null)
            {
                OnWeatherUpdate.Invoke(Current);
            }
        }
    }
}
