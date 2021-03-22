using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WeatherDesktop
{
    public static class ProgramCache
    {
        public static string FilesPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/WeatherDesktop/User/"; } }
        public static void SavePrefs(string filename, string value)
        {
            if (!File.Exists(FilesPath + filename))
            {
                Directory.CreateDirectory(FilesPath);
                var crStream = new FileStream(FilesPath + filename, FileMode.Create);
                crStream.Close();
            }

            File.Delete(FilesPath + filename);
            var stream = new StreamWriter(FilesPath + filename);
            stream.Write(value);
            stream.Close();
        }

        public static string GetPrefs(string filename)
        {
            var stream = new StreamReader(ProgramCache.FilesPath + filename);
            var value = stream.ReadToEnd();
            stream.Close();
            return value;
        }
    }
}
