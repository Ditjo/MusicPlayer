using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Views.NewPlaylist
{
    public class NewPlaylistViewModel : ViewModelBase
    {
        public NewPlaylistViewModel()
        {

        }

        private string? _name;

		public string? Name
		{
			get 
			{
				return _name; 
			}
			set 
			{
				if (_name != value)
				{
					_name = value; 
					OnPropertyChanged();
				}
			}
		}

	}
}
