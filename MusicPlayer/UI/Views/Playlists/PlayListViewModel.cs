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
        public ICommand PlayPlaylistCommand { get; set; }
        public ICommand ShufflePlaylistCommand { get; set; }
        public ICommand QueuePlaylistCommand { get; set; }
        public ICommand DeletePlaylistCommand { get; set; }

        public PlayListViewModel(ListOfPlayLists playlists)
        {
            _listOfplaylists = playlists;

            CreateNewPlaylistCommand = new RelayCommand<object>(OnCreateNewPlaylistCommand, CanCreateNewPlaylistCommand);
            PlayPlaylistCommand = new RelayCommand<object>(OnPlayPlaylistCommand, CanPlayPlaylistCommand);
            ShufflePlaylistCommand = new RelayCommand<object>(OnShufflePlaylistCommand, CanShufflePlaylistCommand);
            QueuePlaylistCommand = new RelayCommand<object>(OnQueuePlaylistCommand, CanQueuePlaylistCommand);
            DeletePlaylistCommand = new RelayCommand<object>(OnDeletePlaylistCommand, CanDeletePlaylistCommand);

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
        }
        private bool CanCreateNewPlaylistCommand(object obj)
        {
            return true;
        }
        private bool CanPlayPlaylistCommand(object arg)
        {
            return true;
        }

        private void OnPlayPlaylistCommand(object obj)
        {
            Debug.WriteLine("Playing Playlist");
            var playlists = Playlists.Where(x => x.Title == obj.ToString());
            if (!playlists.Any()) return;
            var p = playlists.FirstOrDefault();

        }

        private bool CanShufflePlaylistCommand(object arg)
        {
            return true;
        }

        private void OnShufflePlaylistCommand(object obj)
        {
            Debug.WriteLine("Shuffle Playlist");
        }

        private bool CanQueuePlaylistCommand(object arg)
        {
            return true;
        }

        private void OnQueuePlaylistCommand(object obj)
        {
            Debug.WriteLine("Queue Playlist");
        }

        private void OnDeletePlaylistCommand(object obj)
        {
            Playlists.Remove(Playlists.Where(x => x.Title == obj.ToString()).First());
            _listOfplaylists.PlayLists = Playlists.ToList();
            FileHandler.SaveToJSON<ListOfPlayLists>(_listOfplaylists, "playlists.json");
        }
        private bool CanDeletePlaylistCommand(object obj)
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

        //private PlayList _selectedPlaylist;

        //public PlayList SelectedPlaylist
        //{
        //    get 
        //    {
        //        return _selectedPlaylist; 
        //    }
        //    set 
        //    {
        //        if (_selectedPlaylist != value)
        //        {
        //            _selectedPlaylist = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}


        #endregion
    }
}
