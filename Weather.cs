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

        public static void Init() 
        {
            if(Current == null)
            {

            }
        }
    }
}
