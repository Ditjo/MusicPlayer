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
        public AddSongToPlaylistViewModel(List<PlayList> playlists)
        {
            Playlists = playlists.ToObservableCollection();
        }

        public override bool CanConfirmCommand(object obj)
        {
            return SelectedPlaylist != null;
        }
        public override void OnConfirmCommand(object obj)
        {
            Debug.WriteLine("Confirmed");
        }
        public override void OnCancelCommand(object obj)
        {
            OnRequestForNavigation("view_songs", null);
        }

        private ObservableCollection<PlayList> _playlists;
        public ObservableCollection<PlayList> Playlists
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

        private PlayList _selectedPlaylist;

        public PlayList SelectedPlaylist
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
