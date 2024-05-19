using MusicPlayer.Data.Models;
using MusicPlayer.Data.Repositories;
using MusicPlayer.Services.Command;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.UI.Views.Options
{
    public class OptionsViewModel : ViewModelBase
    {
        private static string staticPath = "";

        FileWorker fileWorker;

        public ICommand ConvertCommand { get; set; }

        public OptionsViewModel()
        {
            staticPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            fileWorker = new(staticPath);

            ConvertCommand = new RelayCommand(OnConvertCommand);

            LoadData();
        }

        #region Commands

        public void LoadData()
        {
            Songs = fileWorker.GetAllSongs();
        }

        public void OnConvertCommand()
        {
            Debug.WriteLine("Convert Begin");

            if (Songs == null || Songs.Count == 0) return;

            foreach (var song in Songs)
            {
                var file = TagLib.File.Create(song.Url);
                var fileName = Path.GetFileNameWithoutExtension(song.Url);
                if (fileName != null)
                {
                    string[] parts = fileName.Split(new char[] { '-' }, 2);

                    if (parts.Length == 2)
                    {
                        if (file.Tag.Title is null)
                            file.Tag.Title = parts[1].Trim();

                        if (file.Tag.Performers is null || file.Tag.Performers.Length == 0)
                            file.Tag.Performers = [parts[0].Trim()];

                        file.Save();
                    }

                } 

            }
            Debug.WriteLine("Convert End");

        }


        #endregion

        #region Properties

        private List<Song>? _songs;

        public List<Song>? Songs
        {
            get
            {
                return _songs;
            }
            set
            {
                if (_songs != value)
                {
                    _songs = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion
    }
}
