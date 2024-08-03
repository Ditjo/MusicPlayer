using MusicPlayer.Data.Models;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Base;
using MusicPlayer.UI.Common.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Views.AddSongToPlaylist
{
    public class AddSongToPlaylistViewModel : DialogBase
    {
        private readonly string playlistPath = "playlists.json";
        private readonly Song _selectedSong;
        public AddSongToPlaylistViewModel(List<PlayList> playlists, Song song)
        {
            Playlists = playlists.ToObservableCollection();
            _selectedSong = song;
        }

        public override bool CanConfirmCommand(object obj)
        {
            return SelectedPlaylist != null;
        }
        public override void OnConfirmCommand(object obj)
        {
            Debug.WriteLine("Confirmed");
            if (SelectedPlaylist == null || Playlists == null) return;
            var pl = Playlists.First(x => x.Title == SelectedPlaylist.Title);
            if (pl != null)
            {
                pl.Songs.Enqueue(_selectedSong);
                ListOfPlayLists listOfPlayLists = new ListOfPlayLists() { PlayLists = Playlists.ToList() };
                FileHandler.SaveToJSON<ListOfPlayLists>(listOfPlayLists, playlistPath);

            }
            OnCancelCommand(obj);
        }
        public override void OnCancelCommand(object obj)
        {
            OnRequestForNavigation("view_songs", null);
        }

        private ObservableCollection<PlayList>? _playlists;
        public ObservableCollection<PlayList>? Playlists
        {
            get 
            {
                return _playlists; 
            }
            set 
            {
                if(_playlists != value)
                {
                    _playlists = value;
                    OnPropertyChanged();
                }
            }
        }

        private PlayList? _selectedPlaylist;
        public PlayList? SelectedPlaylist
        {
            get 
            {
                return _selectedPlaylist; 
            }
            set 
            {
                if(_selectedPlaylist != value)
                {
                    _selectedPlaylist = value; 
                    OnPropertyChanged();
                }
            }
        }


    }
}
