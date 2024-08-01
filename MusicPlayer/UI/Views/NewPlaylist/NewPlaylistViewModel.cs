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
        private List<PlayList> _playlists;
        public ICommand CreateNewPlaylistCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NewPlaylistViewModel(List<PlayList> playlists)
        {
            _playlists = playlists;

            CreateNewPlaylistCommand = new RelayCommand<object>(OnCreateNewPlaylistCommand, CanCreateNewPlaylistCommand);
            CancelCommand = new RelayCommand<object>(OnCancelCommand);
        }

        private void OnCreateNewPlaylistCommand(object obj)
        {
            if (obj == null) return;
            PlayList playList = new PlayList(obj.ToString());
            _playlists.Add(playList);
            ListOfPlayLists list = new ListOfPlayLists() { PlayLists = _playlists };
            FileHandler.SaveToJSON<ListOfPlayLists>(list, "playlists.json");

            OnRequestForNavigation("view_playlist", null);
        }
        private bool CanCreateNewPlaylistCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Name) 
                && !(_playlists.Where(x => x.Title == Name).Any());
        }

        private void OnCancelCommand(object obj)
        {
            OnRequestForNavigation("view_playlist", null);
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
