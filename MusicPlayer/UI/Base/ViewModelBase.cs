using MusicPlayer.Data.Models;
using MusicPlayer.Services.Helpers;
using MusicPlayer.Services.Navigation;
using MusicPlayer.UI.Views.MainView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IRequestForNavigationEvent
    {
        private string statePath = "state.json";

        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<string, object>? RequestForNavigationEvent;

        public event EventHandler<PlayList>? PlayPlaylistsEvent;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRequestForNavigation(string request, object? obj)
        {
            RequestForNavigationEvent?.Invoke(request, obj);
        }

        protected virtual void OnRequestForPlayPlaylist(PlayList playlist)
        {
            PlayPlaylistsEvent?.Invoke(this, playlist);
        }

        protected void SaveStateOfMusicPlayer(MusicPlayerState state)
        {
            FileHandler.SaveToJSON<MusicPlayerState>(state, statePath);
        }

        private string _header;
        public string Header
        {
            get 
            {
                return _header;
            }
            set 
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged();
                }
            }
        }

    }



}
