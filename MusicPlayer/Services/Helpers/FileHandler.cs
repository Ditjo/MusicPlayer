using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Services.Helpers
{
    public class FileHandler
    {
        private static string _fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static void SaveToJSON<T>(T obj, string filename)
        {
            var fullPath = Path.Combine(_fileLocation, filename);

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fullPath, json);
        }

        public static T? LoadFromJSON<T>(string filename)
        {
            var fullPath = Path.Combine(_fileLocation, filename);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default;
        }

    }
}
