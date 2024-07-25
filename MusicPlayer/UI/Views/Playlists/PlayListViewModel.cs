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
using System.Collections.ObjectModel;

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
            Playlists = _listOfplaylists.PlayLists.ToObservableCollection();
        }

        #region Commands

        private void OnCreateNewPlaylistCommand(object obj)
        {
            OnRequestForNavigation("dialog_newplaylist", null);

            //if (obj == null) return;
            //PlayList playList = new PlayList(obj.ToString());
            //Playlists.PlayLists.Add(playList);
            //FileHandler.SaveToJSON<ListOfPlayLists>(Playlists, "playlists.json");
        }
        private bool CanCreateNewPlaylistCommand(object obj)
        {
            return true;
        }

        #endregion

        #region Properties

        private ObservableCollection<PlayList> _playlists;
        public ObservableCollection<PlayList> Playlists
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
