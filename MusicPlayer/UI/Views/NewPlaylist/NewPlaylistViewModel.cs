using MusicPlayer.Data.Models;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.UI.Views.NewPlaylist
{
    public class NewPlaylistViewModel : ViewModelBase
    {
        private ListOfPlayLists _listOfplaylists;
        public ICommand CreateNewPlaylistCommand { get; set; }

        public NewPlaylistViewModel(ListOfPlayLists playlists)
        {
            _listOfplaylists = playlists;

            CreateNewPlaylistCommand = new RelayCommand<object>(OnCreateNewPlaylistCommand, CanCreateNewPlaylistCommand);
        }

        private void OnCreateNewPlaylistCommand(object obj)
        {
            if (obj == null) return;
            PlayList playList = new PlayList(obj.ToString());
            _listOfplaylists.PlayLists.Add(playList);
            FileHandler.SaveToJSON<ListOfPlayLists>(_listOfplaylists, "playlists.json");

            OnRequestForNavigation("view_playlist", null);
        }
        private bool CanCreateNewPlaylistCommand(object obj)
        {
            //return !string.IsNullOrWhiteSpace(Name);
            return true;
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
