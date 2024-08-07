using MusicPlayer.Data.Models;
using MusicPlayer.Data.Repositories;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Helpers;
using MusicPlayer.Services.Navigation;
using MusicPlayer.UI.Base;
using MusicPlayer.UI.Common.Dialog;
using MusicPlayer.UI.Views.AddSongToPlaylist;
using MusicPlayer.UI.Views.FrontPage;
using MusicPlayer.UI.Views.NewPlaylist;
using MusicPlayer.UI.Views.Options;
using MusicPlayer.UI.Views.Playlists;
using MusicPlayer.UI.Views.PlaylistSongs;
using MusicPlayer.UI.Views.SongControls;
using MusicPlayer.UI.Views.Songs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace MusicPlayer.UI.Views.MainView
{
    internal class MainViewModel : ViewModelBase
    {
        private static string staticPath = "";
        private MusicPlayerState _state;
        private List<PlayList> _playlists;
        private DialogWindow _dialogWindow;
        FileWorker fileWorker;

        public ICommand NavigationCommand { get; set; }
        public ICommand AddSongToQueueCommand { get; set; }
        public ICommand AddSongToPlayNowCommand { get; set; }

        public MainViewModel(MusicPlayerState state, List<PlayList> playlists)
        {
            _state = state;
            _playlists = playlists;

            staticPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            fileWorker = new(staticPath);

            LoadData();

            NavigationCommand = new RelayCommand<object>(OnNavigateCommand);
            AddSongToQueueCommand = new RelayCommand(OnAddSongToQueueCommand);
            AddSongToPlayNowCommand = new RelayCommand(OnAddSongToPlayNowCommand);

            SongControls = new SongControlsViewModel(_state);

            //Sets Starting Page
            RequestForNavigation("View_Home");
        }

        public void LoadData()
        {
            FullSongList = fileWorker.GetAllSongs();
        }

        internal void RequestForNavigation(string obj, object? entity = null)
        {
            //Used To Also Close Dialogs
            if (_dialogWindow != null)
            {
                _dialogWindow.Close();
            }
            Navigation(obj, entity);
        }

        internal void Navigation(string obj, object? entity = null)
        {
            ViewModelBase viewModel;
            //SelectedViewModel = null;
            var navi = obj.ToLower().Split("_");
            if (navi[0] == "view")
            {

                switch (navi[1])
                {
                    case "home":
                        viewModel = new FrontPageViewModel();
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        viewModel.Header = "Home";
                        SelectedViewModel = viewModel;
                        break;
                    case "playlist":
                        viewModel = new PlayListViewModel(_playlists);
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        viewModel.PlayPlaylistsEvent += RequestForPlayPlaylist;
                        viewModel.Header = "Playlists";
                        SelectedViewModel = viewModel;
                        break;
                    case "songs":
                        viewModel = new SongViewModel(FullSongList, _playlists);
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        viewModel.Header = "Songs";
                        SelectedViewModel = viewModel;
                        break;
                    case "playlistsongs":
                        viewModel = new PlayListSongViewModel(_playlists.Where(x => x.Title == entity.ToString()).First());
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        viewModel.Header = "Songs";
                        SelectedViewModel = viewModel;
                        break;
                    case "options":
                        viewModel = new OptionsViewModel();
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        viewModel.Header = "Options";
                        SelectedViewModel = viewModel;
                        break;
                    default: throw new ArgumentException();

                }
            }
            else if (navi[0] == "dialog")
            {
                switch (navi[1])
                {
                    case "newplaylist":
                        _dialogWindow = new DialogWindow();
                        _dialogWindow.Title = "New Playlist";
                        viewModel = new NewPlaylistViewModel(_playlists);
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        _dialogWindow.DialogContent.Content = viewModel;
                        _dialogWindow.ShowDialog();
                        break;
                    case "addsongtoplaylist":
                        _dialogWindow = new DialogWindow();
                        _dialogWindow.Title = "Add To Playlist";
                        viewModel = new AddSongToPlaylistViewModel(_playlists, SelectedSong);
                        viewModel.RequestForNavigationEvent += RequestForNavigation;
                        _dialogWindow.DialogContent.Content = viewModel;
                        _dialogWindow.ShowDialog();
                        break;
                    default : throw new ArgumentException();
                }
            }
        }

        internal void RequestForPlayPlaylist(object? sender, PlayList playlist)
        {
            Debug.WriteLine("Im here");
            OnAddPlaylistToPlayNowCommand(playlist);
        }

        #region Commands
        private void OnNavigateCommand(object obj)
        {
            RequestForNavigation(obj.ToString());
        }

        private void OnAddSongToQueueCommand()
        {
            if (SelectedSong is not null)
            {
                SongControls.AddToQueue(SelectedSong);
            }
        }
        private void OnAddSongToPlayNowCommand()
        {
            if (SelectedSong is not null)
            {
                SongControls.PlayNow(SelectedSong);
            }
        }
        private void OnAddPlayListToQueueCommand(PlayList playlist)
        {
            if (playlist is not null && playlist.Songs.Count != 0)
            {
                //SongControls.AddPlayListToQueue(SelectedSong);
            }
        }
        private void OnAddPlaylistToPlayNowCommand(PlayList playlist)
        {
            if (playlist is not null && playlist.Songs.Count != 0)
            {
                SongControls.PlayNow(playlist.Songs);
            }
        }

        #endregion

        #region Properties

        private ViewModelBase _selectedViewModel;
        public ViewModelBase SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                if (value != _selectedViewModel)
                {
                    _selectedViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private SongControlsViewModel _songControls;
        public SongControlsViewModel SongControls
        {
            get
            {
                return _songControls;
            }
            set
            {
                if (value != _songControls)
                {
                    _songControls = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Song>? _fullSongList;
        public List<Song>? FullSongList
        {
            get
            {
                return _fullSongList;
            }
            set
            {
                if (_fullSongList != value)
                {
                    _fullSongList = value;
                    OnPropertyChanged();
                }
            }
        }

        private Song? _selectedSong;
        public Song? SelectedSong
        {
            get
            {
                return _selectedSong;
            }
            set
            {
                if (_selectedSong != value)
                {
                    _selectedSong = value;
                    OnPropertyChanged();
                }
            }
        }

        //TODO: Update this when SongQueue is updated
        public ObservableCollection<Song> SongQueueObs
        {
            get { return SongControls.SongQueue.ToList().ToObservableCollection(); }
        }


        #endregion

        //#region ViewModels

        //private FrontPageViewModel frontPageViewModel;
        //public FrontPageViewModel FrontPageViewModel
        //{
        //    get
        //    {
        //        if (frontPageViewModel is null)
        //            frontPageViewModel = new FrontPageViewModel();
        //        return frontPageViewModel;
        //    } 
        //}

        //private FrontPageViewModel frontPageViewModel;
        //public FrontPageViewModel FrontPageViewModel
        //{
        //    get
        //    {
        //        if (frontPageViewModel is null)
        //            frontPageViewModel = new FrontPageViewModel();
        //        return frontPageViewModel;
        //    }
        //}

        //#endregion
    }
}
