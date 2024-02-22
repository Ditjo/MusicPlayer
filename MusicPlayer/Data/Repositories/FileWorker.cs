using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Data.Repositories
{
    internal class FileWorker
    {
        public List<string> ReadfilePaths()
        {
            var files = Directory.GetFiles("../../../MockData");

            if(files != null)
                return files.ToList();
            return new();
        }
    }
}
