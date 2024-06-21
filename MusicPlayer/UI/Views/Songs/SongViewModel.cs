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
		private List<Song>? _song;

		public List<Song>? MyProperty
		{
			get 
			{
				return _song; 
			}
			set 
			{
				if (_song != value)
				{
					_song = value;
					OnPropertyChanged();
				}
			}
		}

	}
}
