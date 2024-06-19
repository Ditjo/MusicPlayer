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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace MusicPlayer.UI.Views.MainView
{
    internal class MainViewModel : ViewModelBase
    {
        private bool musicNotStoppedByPerson = true;
        private WaveOutEvent? outputDevice;
        private readonly DispatcherTimer musicTimer;
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
            RequestForNavigation("Playlist");

            musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromMilliseconds(500);
            musicTimer.Tick += TimerTick;

            OutputVolume = 100;
            PlayBtn = "Play";
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
                case "FrontPage":
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

        public void OnClosingCommand(object? sender, CancelEventArgs e)
        {
            Debug.WriteLine("Closing Progam");
            CleanPlayback();
        }

        public void PlayMusic()
        {
            Debug.WriteLine("Playing Music...");
            if (outputDevice == null) return;

            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                musicNotStoppedByPerson = false;
                outputDevice?.Stop();
                musicTimer.Stop();
                PlayBtn = "Play";
            }
            else
            {
                outputDevice?.Play();
                musicTimer.Start();
                PlayBtn = "Stop";
            }
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
            musicTimer.Stop();
        }

        public void OnRewindMusicCommand()
        {
            if (audioFile == null) return;
            audioFile.Position = 0;
        }

        public void OnSkipTimeInMusicCommand(object sec)
        {
            if (audioFile != null && sec != null)
            {
                audioFile.Skip(int.Parse(sec.ToString()));
            }
        }

        public void SetTime(TimeSpan time)
        {
            if (audioFile != null)
            {
                audioFile.CurrentTime = time;
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
            PlayBtn = "Play";
            musicNotStoppedByPerson = true;
        }

        //
        public void CleanPlayback()
        {

            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.PlaybackStopped -= OnPlaybackStopped;
                    PlayBtn = "Play";
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

        public void SelectedSongInit()
        {
            CleanPlayback();
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(SelectedMusic?.Url);
                outputDevice.Init(audioFile);
            }
            TimeOffSong = audioFile is null ? TimeSpan.Zero : audioFile.TotalTime;
        }


        public bool isDragging = false;
        private void TimerTick(object sender, EventArgs e)
        {
            if (audioFile is not null && !isDragging)
            {
                CurrentTime = audioFile.CurrentTime;
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
                    _selectedMusic = value;
                    if (value is not null) SelectedSongInit();
                    OnPropertyChanged();
                }
            }
        }

        private string _playBtn;

        public string PlayBtn
        {
            get
            {
                return _playBtn;
            }
            set
            {
                if (_playBtn != value)
                {
                    _playBtn = value;
                    OnPropertyChanged();
                }
            }
        }


        private TimeSpan _timeOffSong;
        public TimeSpan TimeOffSong
        {
            get
            {
                return _timeOffSong;
            }
            set
            {
                if (_timeOffSong != value)
                {
                    _timeOffSong = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _currentTime;
        public TimeSpan CurrentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                if (_currentTime != value)
                {

                    _currentTime = value;
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
                if (_outputVolume != value)
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
