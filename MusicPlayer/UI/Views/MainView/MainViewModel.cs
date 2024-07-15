using MusicPlayer.Data.Models;
using MusicPlayer.Data.Repositories;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Helpers;
using MusicPlayer.Services.Navigation;
using MusicPlayer.UI.Base;
using MusicPlayer.UI.Views.FrontPage;
using MusicPlayer.UI.Views.Options;
using MusicPlayer.UI.Views.Playlists;
using MusicPlayer.UI.Views.SongControls;
using MusicPlayer.UI.Views.Songs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        FileWorker fileWorker;

        public ICommand NavigationCommand { get; set; }
        public ICommand AddSongToQueueCommand { get; set; }
        public ICommand AddSongToPlayNowCommand { get; set; }

        public MainViewModel()
        {
            staticPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            fileWorker = new(staticPath);

            LoadData();

            NavigationCommand = new RelayCommand<object>(OnNavigateCommand);
            AddSongToQueueCommand = new RelayCommand(OnAddSongToQueueCommand);
            AddSongToPlayNowCommand = new RelayCommand(OnAddSongToPlayNowCommand);

            SongControls = new SongControlsViewModel();

            //Sets Starting Page
            RequestForNavigation("Home");
        }

        public void LoadData()
        {
            FullSongList = fileWorker.GetAllSongs();
        }

        internal void RequestForNavigation(string obj, object? entity = null)
        {
            //Used To Also Close Dialogs
            Navigation(obj, entity);
        }

        internal void Navigation(string obj, object? entity = null)
        {
            ViewModelBase viewModel;
            switch (obj)
            {
                case "Home":
                    viewModel = new FrontPageViewModel();
                    viewModel.RequestForNavigationEvent += RequestForNavigation;
                    viewModel.Header = "Home";
                    SelectedViewModel = viewModel;
                    break;
                case "Playlist":
                    viewModel = new PlayListViewModel();
                    viewModel.RequestForNavigationEvent += RequestForNavigation;
                    viewModel.Header = "Playlists";
                    SelectedViewModel = viewModel;
                    break;
                case "Songs":
                    viewModel = new SongViewModel(FullSongList);
                    viewModel.RequestForNavigationEvent += RequestForNavigation;
                    viewModel.Header = "Songs";
                    SelectedViewModel = viewModel;
                    break;
                case "Options":
                    viewModel = new OptionsViewModel();
                    viewModel.RequestForNavigationEvent += RequestForNavigation;
                    viewModel.Header = "Options";
                    SelectedViewModel = viewModel;
                    break;
                default: throw new ArgumentException();

            }
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
                SongControls.AddSongToQueue(SelectedSong);
            }
        }
        private void OnAddPlayListToQueueCommand()
        {
            if (SelectedSong is not null)
            {
                //SongControls.AddPlayListToQueue(SelectedSong);
            }
        }
        private void OnAddSongToPlayNowCommand()
        {
            if (SelectedSong is not null)
            {
                SongControls.AddSongToPlayNow(SelectedSong);
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
