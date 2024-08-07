using MusicPlayer.Data.Models;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Views.PlaylistSongs
{
    public class PlayListSongViewModel : ViewModelBase
    {
        private readonly PlayList _playlist;

        public PlayListSongViewModel(PlayList playlist)
        {
            _playlist = playlist;

            Songs = _playlist.Songs.ToObservableCollection();
            PlaylistTitle = _playlist.Title;
        }



        private ObservableCollection<Song>? _songs;
        public ObservableCollection<Song>? Songs
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

        private string? _playlistTitle;
        public string? PlaylistTitle
        {
            get
            {
                return _playlistTitle;
            }
            set
            {
                if (_playlistTitle != value)
                {
                    _playlistTitle = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
