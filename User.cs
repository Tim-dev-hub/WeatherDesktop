using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WeatherDesktop
{
    public static class User
    {
        public static Tim_hub.Weather.API.Weather.Coord Location
        {
            get
            {
                
            }
            set
            {
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string path = docs + "/WeatherDesktop/User/";
                string filename = "Location.json";
                if (!File.Exists(path + filename))
                {
                    Directory.CreateDirectory(path);
                    File.Create(path + filename);
                }
            }
        }
    }
}
