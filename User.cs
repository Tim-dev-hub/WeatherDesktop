using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WeatherDesktop
{
    public static class User
    {

        public static Tim_hub.Weather.API.Weather.Coord Location
        {
            get
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<Tim_hub.Weather.API.Weather.Coord>(ProgramCache.GetPrefs("Location.json"));
                    return obj;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ProgramCache.SavePrefs("Location.json", JsonConvert.SerializeObject(value, Formatting.Indented));
            }
        }

    }
}
