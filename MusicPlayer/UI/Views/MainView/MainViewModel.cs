using MusicPlayer.Services.Command;
using MusicPlayer.Services.Navigation;
using MusicPlayer.UI.Base;
using MusicPlayer.UI.Views.FrontPage;
using MusicPlayer.UI.Views.Options;
using MusicPlayer.UI.Views.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.UI.Views.MainView
{
    internal class MainViewModel : ViewModelBase
    {

        public ICommand NavigationCommand { get; set; }
        //public ICommand OptionCommand { get; set; }
        //public ICommand PlaylistsListCommand { get; set; }
        //public ICommand SongListCommand { get; set; }
        //public ViewModelBase PlayListViewModel { get; set; }
        //public ViewModelBase FrontPageViewModel { get; set; }
        //public ViewModelBase OptionsViewModel { get; set; }
        public MainViewModel()
        {
            //PlayListViewModel = new PlayListViewModel();
            //FrontPageViewModel = new FrontPageViewModel();
            //OptionsViewModel = new OptionsViewModel();

            SelectedViewModel = new FrontPageViewModel();
            InitCommands();
        }

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

        private void InitCommands()
        {
            NavigationCommand = new RelayCommand<object>(OnNavigateCommand);
        }

        internal void RequestForNavigation(string obj, object entity = null)
        {
            //Used To Also Close Dialogs
            Navigation(obj, entity);
        }

        internal void Navigation(string obj, object entity = null)
        {
            switch (obj)
            {
                case "FrontPage":
                    ViewModelBase frontPageView = new FrontPageViewModel();
                    frontPageView.RequestForNavigationEvent += RequestForNavigation;
                    SelectedViewModel = frontPageView;
                    break;
                case "Playlist":
                    ViewModelBase playListView = new PlayListViewModel();
                    playListView.RequestForNavigationEvent += RequestForNavigation;
                    SelectedViewModel = playListView;
                    break;
                case "Options":
                    ViewModelBase optionsView = new OptionsViewModel();
                    optionsView.RequestForNavigationEvent += RequestForNavigation;
                    SelectedViewModel = optionsView;
                    break;
                default: throw new ArgumentException();
                
            }
        }

        #region Commands
        private void OnNavigateCommand(object obj)
        {
            RequestForNavigation(obj.ToString());
        }




        #endregion
    }
}
