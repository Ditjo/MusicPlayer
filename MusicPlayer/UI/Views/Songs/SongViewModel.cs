using MusicPlayer.Data.Models;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Views.Songs
{
    public class SongViewModel : ViewModelBase
    {
		private readonly List<Song>? _fullSongList;
		private readonly List<PlayList>? _fullPlaylists;

		public SongViewModel(List<Song>? songs, List<PlayList> playlists) 
		{
            _fullSongList = songs ?? new List<Song>();
            _fullPlaylists = playlists ?? new List<PlayList>();

			LoadData();
        }

		private void LoadData()
		{
			VisibleSongs = _fullSongList;
			Playlists = _fullPlaylists;

        }

        private List<Song>? _visibleSongs;
		public List<Song>? VisibleSongs
		{
			get
			{
				return _visibleSongs;
			}
			set
			{
				if (_visibleSongs != value)
				{
					_visibleSongs = value;
					OnPropertyChanged();
				}
			}
		}

		private List<PlayList> _playlists;
		public List<PlayList> Playlists
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


	}
}
