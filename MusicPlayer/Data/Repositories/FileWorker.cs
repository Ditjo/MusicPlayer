using MusicPlayer.Data.Models;
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
        private string _filePath;
        public FileWorker(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> ReadfilePaths()
        {
            var files = Directory.GetFiles(_filePath);

            if(files != null)
                return files.ToList();
            return new();
        }

        public List<Song> ReadSongs()
        {
            List<Song> list = new List<Song>();
            var files = Directory.GetFiles(_filePath);

            foreach(var song in files)
            {
                list.Add(new Song
                {
                    Url = song,
                });
            }
            return list;
        }
    }
}
