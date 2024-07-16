using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Services.Helpers
{
    public class FileHandler
    {
        //TODO: foldername needs to be set in a different way, so as to make it more flexable.
        private static string foldername = "MusicPlayer";
        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string folderLocation = "";

        private static void CheckIfFolderExist()
        {
            folderLocation = Path.Combine(appdata, foldername);
            if (!Directory.Exists(folderLocation)) 
            {
                Directory.CreateDirectory(folderLocation);
            }
        }

        public static void SaveToJSON<T>(T obj, string filename)
        {
            CheckIfFolderExist();
            var fullPath = Path.Combine(folderLocation, filename);

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fullPath, json);
        }

        public static T? LoadFromJSON<T>(string filename)
        {
            CheckIfFolderExist();
            var fullPath = Path.Combine(folderLocation, filename);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default;
        }

    }
}
