using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WeatherDesktop.Packs
{
    public class WeatherPack
    {
        protected string DirectoryPath;

        public WeatherPack(string path)
        {
            DirectoryPath = path;
        }

        public string GetWallpaper(Tim_hub.Weather.API.Weather weather)
        {
            StreamReader reader = new StreamReader(DirectoryPath + "/config.json");
            string config = reader.ReadToEnd();
            reader.Close();
            List<WeatherWallpaper> wallpapers = JsonConvert.DeserializeObject<List<WeatherWallpaper>>(config);

            foreach(var wallp in wallpapers)
            {
                var value = GetWeatherValue(wallp.Condition.PropertyReference, weather);
                Console.WriteLine($"{value}   {wallp.Condition.Operator}    {wallp.Condition.Value}      is {wallp.Condition.Suitable(value)}");
                if (wallp.Condition.Suitable(value))
                {
                    return DirectoryPath +"\\"+ wallp.WallpaperFilename;
                }
            }
            throw new Exception("Wallpaper pack don't valid");
        }

        private string GetWeatherValue(string fieldReference, Tim_hub.Weather.API.Weather weather)
        {
            string[] propnames = fieldReference.Split('.');

            Type lastObj = weather.GetType();
            object fieldValue = null;

            foreach (var propname in propnames)
            {
                bool isArray = propname.Split('[').Length >= 2;
                int arrayIndex = isArray ? int.Parse(propname.Split('[')[1].Replace("]", "")) : 0;
                var field = lastObj.GetField(isArray ? propname.Replace("[" + arrayIndex + "]", "") : propname);
                lastObj = field.FieldType;


                if (isArray)
                {
                    lastObj = ((dynamic[])field.GetValue(fieldValue ?? weather))[arrayIndex].GetType();
                    fieldValue = ((dynamic[])field.GetValue(fieldValue ?? weather))[arrayIndex];
                }
                else
                {
                    fieldValue = field.GetValue(fieldValue ?? weather);
                }
            }

            return fieldValue.ToString();
        }

        private class WeatherWallpaper
        {
            public string WallpaperFilename;
            public BooleanCondition Condition;

            public WeatherWallpaper() { }
            public WeatherWallpaper(string name, BooleanCondition cond)
            {
                WallpaperFilename = name;
                Condition = cond;
            }
        }

        private class BooleanCondition
        {
            public string Operator;
            public string Value;
            public string PropertyReference;

            public bool Suitable(string value)
            {
                //someObject.someObject.property
                dynamic fValue = this.Value;
                dynamic sValue = value;

                switch (Operator)
                {
                    case ">":
                        return fValue > sValue;
                        break;
                    case "<":
                        return fValue < sValue;
                        break;
                    case "==":
                        return fValue == sValue;
                        break;
                    case "!=":
                        return fValue != sValue;
                        break;
                    case ">=":
                        return fValue >= sValue;
                        break;
                    case "<=":
                        return fValue <= sValue;
                        break;
                }
                throw new Exception("Operator dont aviable");
            }

            public BooleanCondition(string propRefer, string oper, string val)
            {
                Operator = oper;
                Value = val;
                PropertyReference = propRefer;
            }

            public BooleanCondition() { }
        }

       /* public void Main()
        {
            List<WeatherWallpaper> wallpapers = new List<WeatherWallpaper>();

            // Weather.Current.Condition[0].IconID
            wallpapers.Add(new WeatherWallpaper("day_clear.jpg", new BooleanCondition("Condition[0].IconID", "==", "01d")));
            wallpapers.Add(new WeatherWallpaper("day_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "02d")));
            wallpapers.Add(new WeatherWallpaper("day_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "03d")));
            wallpapers.Add(new WeatherWallpaper("day_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "04d")));
            wallpapers.Add(new WeatherWallpaper("day_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "09d")));
            wallpapers.Add(new WeatherWallpaper("day_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "10d")));
            wallpapers.Add(new WeatherWallpaper("day_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "11d")));
            wallpapers.Add(new WeatherWallpaper("day_snow.jpg", new BooleanCondition("Condition[0].IconID", "==", "13d")));
            wallpapers.Add(new WeatherWallpaper("day_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "50d")));

            wallpapers.Add(new WeatherWallpaper("night_clear.jpg", new BooleanCondition("Condition[0].IconID", "==", "01n")));
            wallpapers.Add(new WeatherWallpaper("night_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "02n")));
            wallpapers.Add(new WeatherWallpaper("night_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "03n")));
            wallpapers.Add(new WeatherWallpaper("night_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "04n")));
            wallpapers.Add(new WeatherWallpaper("night_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "09n")));
            wallpapers.Add(new WeatherWallpaper("night_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "10n")));
            wallpapers.Add(new WeatherWallpaper("night_rain.jpg", new BooleanCondition("Condition[0].IconID", "==", "11n")));
            wallpapers.Add(new WeatherWallpaper("night_snow.jpg", new BooleanCondition("Condition[0].IconID", "==", "13n")));
            wallpapers.Add(new WeatherWallpaper("night_clouds.jpg", new BooleanCondition("Condition[0].IconID", "==", "50n")));

            Console.WriteLine(JsonConvert.SerializeObject(wallpapers, Formatting.Indented));
        }*/
    }
}
