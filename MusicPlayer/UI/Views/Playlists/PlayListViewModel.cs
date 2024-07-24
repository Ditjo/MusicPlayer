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
using MusicPlayer.Services.Helpers;

namespace MusicPlayer.UI.Views.Playlists
{
    public class PlayListViewModel : ViewModelBase
    {
        private ListOfPlayLists _listOfplaylists;

        public ICommand CreateNewPlaylistCommand { get; set; }

        public PlayListViewModel(ListOfPlayLists playlists)
        {
            _listOfplaylists = playlists;

            CreateNewPlaylistCommand = new RelayCommand<object>(OnCreateNewPlaylistCommand, CanCreateNewPlaylistCommand);

            InitPlaylists();
        }

        private void InitPlaylists()
        {
            Playlists = _listOfplaylists;
        }

        #region Commands

        private void OnCreateNewPlaylistCommand(object obj)
        {
            if(obj == null) return;
            PlayList playList = new PlayList(obj.ToString());
            Playlists.PlayLists.Add(playList);
            FileHandler.SaveToJSON<ListOfPlayLists>(Playlists, "playlists.json");
        }
        private bool CanCreateNewPlaylistCommand(object obj)
        {
            return true;
        }

        #endregion

        #region Properties

        private ListOfPlayLists _playlists;
        public ListOfPlayLists Playlists
        {
            get 
            {
                return _playlists; 
            }
            set 
            {
                if (_playlists != value)
                {
                    _playlists = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion
    }
}
