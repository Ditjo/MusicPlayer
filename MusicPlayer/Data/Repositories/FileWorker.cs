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

        public List<Song> GetAllSongs()
        {
            List<Song> songs = new List<Song>();

            songs.AddRange(GetSongs(_filePath));

            return songs;
        }

        private string[] GetFolders(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Recursive Method: Calls it's self
        private List<Song> GetSongs(string path)
        {
            List<Song> list = new List<Song>();

            list.AddRange(GetSongObject(path));
            
            foreach (var subFolder in GetFolders(path))
            {
                list.AddRange(GetSongs(subFolder));
            }

            return list;
        }


        private List<Song> GetSongObject(string path)
        {
            List<Song> list = new List<Song>();

            try
            {
                var songs = Directory.GetFiles(path);

                foreach (var song in songs)
                {
                    list.Add(new Song()
                    {
                        Url = song
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }
    }
}
