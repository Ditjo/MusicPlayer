using MusicPlayer.Data.Repositories;
using MusicPlayer.Services.Command;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MusicPlayer.UI.Views.Playlists
{
    public class PlayListViewModel : ViewModelBase
    {

        private static string staticPath = "";

        FileWorker fileWorker;

        public PlayListViewModel()
        {
            staticPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            staticPath += "\\Music\\ACDC";

            fileWorker = new(staticPath);

            LoadData();
        }

        #region Commands

        public void LoadData()
        {
            FilePaths = fileWorker.ReadfilePaths();
        }

        #endregion

        #region Properties
        private List<string> _filePaths;
        public List<string> FilePaths
        {
            get
            {
                return _filePaths;
            }
            set
            {
                if (_filePaths != value)
                {
                    _filePaths = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
