using MusicPlayer.Data.Models;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Navigation;
using MusicPlayer.UI.Base;
using MusicPlayer.UI.Views.FrontPage;
using MusicPlayer.UI.Views.Options;
using MusicPlayer.UI.Views.Playlists;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.UI.Views.MainView
{
    internal class MainViewModel : ViewModelBase
    {
        private bool musicNotStoppedByPerson = true;
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        public ICommand NavigationCommand { get; set; }
        public ICommand PlayMusicCommand { get; set; }
        public ICommand StopMusicCommand { get; set; }
        public ICommand RewindMusicCommand { get; set; }
        public ICommand SkipTimeInMusicCommand { get; set; }

        public MainViewModel()
        {
            NavigationCommand = new RelayCommand<object>(OnNavigateCommand);
            PlayMusicCommand = new RelayCommand(PlayMusic, CanPlayMusic);
            StopMusicCommand = new RelayCommand(StopMusic);
            RewindMusicCommand = new RelayCommand(OnRewindMusicCommand);
            SkipTimeInMusicCommand = new RelayCommand<object>(OnSkipTimeInMusicCommand);

            //Sets Starting Page
            Navigation("Playlist");

            OutputVolume = 100;
        }

        internal void RequestForNavigation(string obj, object? entity = null)
        {
            //Used To Also Close Dialogs
            Navigation(obj, entity);
        }

        internal void Navigation(string obj, object? entity = null)
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

        public void OnClosingCommand(object? sender, CancelEventArgs e)
        {
            Debug.WriteLine("Closing Progam");
            CleanPlayback();
        }

        public void PlayMusic()
        {
            Debug.WriteLine("Playing Music...");

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(SelectedMusic.Url);
                outputDevice.Init(audioFile);
            }

            outputDevice.Play();
        }

        public bool CanPlayMusic()
        {
            return SelectedMusic is not null && File.Exists(SelectedMusic.Url);
        }

        public void StopMusic()
        {
            Debug.WriteLine("Stopped Music");
            musicNotStoppedByPerson = false;
            outputDevice?.Stop();
        }

        public void OnRewindMusicCommand()
        {
            if (audioFile == null) return;
            audioFile.Position = 0;
        }

        public void OnSkipTimeInMusicCommand(object sec)
        {
            if(audioFile != null && sec != null)
            {
                audioFile.Skip(int.Parse(sec.ToString()));
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            //If music stopped on it's own. CleanPlayback.
            //If more in playlist play that.
            if (musicNotStoppedByPerson)
            {
                CleanPlayback();
            }
            musicNotStoppedByPerson = true;
        }

        public void CleanPlayback()
        {

            if (outputDevice != null)
            {
                if(outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Stop();
                }
                outputDevice.Dispose();
                outputDevice = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
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

        private Song? _selectedMusic;
        public Song? SelectedMusic
        {
            get
            {
                return _selectedMusic;
            }
            set
            {
                if (_selectedMusic != value)
                {
                    CleanPlayback();
                    _selectedMusic = value;
                    OnPropertyChanged();
                }
            }
        }

        private float _outputVolume;

        public float OutputVolume
        {
            get 
            {
                return _outputVolume;
            }
            set 
            {
                if(_outputVolume != value)
                {
                    //if(audioFile != null)
                    //{
                    //    audioFile.Volume = value / 100f;
                    //}

                    if (outputDevice != null)
                    {
                        outputDevice.Volume = value / 100f;
                    }

                    _outputVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

    }
}
