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

	}
}
