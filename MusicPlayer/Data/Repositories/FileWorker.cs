using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using MusicPlayer.Data.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MusicPlayer.Data.Repositories
{
    internal class FileWorker
    {
        private string _filePath;
        private List<string> errorSongs = new();
        public FileWorker(string filePath)
        {
            _filePath = filePath;
        }

        public List<Song> GetAllSongs()
        {
            List<Song> songs = new List<Song>();

            songs.AddRange(GetSongs(_filePath));

            System.IO.File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.txt"), errorSongs);

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
                var songs = Directory.GetFiles(path, "*.mp3");

                foreach (var song in songs)
                {
                    TagLib.File tfile;
                    Debug.WriteLine(song);
                    try
                    {
                        tfile = TagLib.File.Create(song);

                        list.Add(new Song()
                        {
                            Url = song,
                            SongName = tfile.Tag.Title != null ? tfile.Tag.Title : Path.GetFileNameWithoutExtension(song),
                            Artist = tfile.Tag.Performers.Length != 0 ? tfile.Tag.Performers.First() : "N/A",
                            Length = tfile.Properties.Duration
                        });
                    }
                    catch (CorruptFileException e)
                    {
                        //TODO: Need to be able to see in program that there are problems with some files and where to find log file.
                        Debug.WriteLine("Error: The file is not a valid MP3 file or is corrupted.");
                        Debug.WriteLine(e.Message);
                        errorSongs.Add(song);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("An unexpected error occurred:");
                        Debug.WriteLine(e.Message);
                        errorSongs.Add(song);
                    }
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
