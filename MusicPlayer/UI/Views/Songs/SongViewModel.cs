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

		public SongViewModel(List<Song>? songs) 
		{
            _fullSongList = songs ?? new List<Song>();

			LoadData();
        }

		private void LoadData()
		{
			VisibleSongs = _fullSongList;
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

	}
}
