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
using MusicPlayer.Data.Models;

namespace MusicPlayer.UI.Views.Playlists
{
    public class PlayListViewModel : ViewModelBase
    {
        //private static string staticPath = "";

        //FileWorker fileWorker;

        public PlayListViewModel()
        {
            //staticPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            //fileWorker = new(staticPath);

            //LoadData();
        }

        #region Commands

        //public void LoadData()
        //{
        //    Songs = fileWorker.GetAllSongs();
        //}

        #endregion

        #region Properties

        //private List<Song>? _songs;

        //public List<Song>? Songs
        //{
        //    get 
        //    {
        //        return _songs; 
        //    }
        //    set 
        //    {
        //        if (_songs != value)
        //        {
        //            _songs = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}


        #endregion
    }
}
