using MusicPlayer.Data.Models;
using MusicPlayer.Services.Command;
using MusicPlayer.UI.Base;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TagLib.Mpeg;

namespace MusicPlayer.UI.Views.SongControls
{
    public class SongControlsViewModel : ViewModelBase
    {
        private bool musicNotStoppedByPerson = true;
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        private readonly DispatcherTimer musicTimer;

        public ICommand NavigationCommand { get; set; }
        public ICommand PlayMusicCommand { get; set; }
        public ICommand StopMusicCommand { get; set; }
        public ICommand RewindMusicCommand { get; set; }
        public ICommand SkipTimeInMusicCommand { get; set; }

        public SongControlsViewModel()
        {
            PlayMusicCommand = new RelayCommand(PlayMusic, CanPlayMusic);
            StopMusicCommand = new RelayCommand(StopMusic);
            RewindMusicCommand = new RelayCommand(OnRewindMusicCommand);
            SkipTimeInMusicCommand = new RelayCommand<object>(OnSkipTimeInMusicCommand);

            musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromMilliseconds(500);
            musicTimer.Tick += TimerTick;

            SongQueue = new();

            OutputVolume = 100;
            PlayBtn = "Play";
        }

        #region Commands

        private void PlayMusic()
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

        private bool CanPlayMusic()
        {
            return SongPlaying is not null && System.IO.File.Exists(SongPlaying.Url);
        }

        private void StopMusic()
        {
            Debug.WriteLine("Stopped Music");

            musicNotStoppedByPerson = false;
            outputDevice?.Stop();
            musicTimer.Stop();
        }

        private void OnRewindMusicCommand()
        {
            if (audioFile == null) return;
            audioFile.Position = 0;
        }

        private void OnSkipTimeInMusicCommand(object sec)
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

        public bool isDragging = false;
        private void TimerTick(object sender, EventArgs e)
        {
            if (audioFile is not null && !isDragging)
            {
                CurrentTime = audioFile.CurrentTime;
            }
        }

        #endregion

        #region SongQueueControls

        //Below Commands used to set song and start next song.

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (musicNotStoppedByPerson)
            {
                PlayNextSongFromQueue();
            }
            else
            {
                PlayBtn = "Play";
                musicNotStoppedByPerson = true;
            }
        }

        private void PlayNextSongFromQueue()
        {
            CleanPlayback();
            if (SongQueue is null || SongQueue.Count == 0) return;

            SongPlaying = SongQueue.Dequeue();
            //TODO: Add Song to Collection of Songs that have been played.
            InitSongToPlay(SongPlaying);
            PlayMusic();
        }

        private void CleanPlayback()
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

        private void InitSongToPlay(Song song)
        {
            CleanPlayback();
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(song?.Url);
                outputDevice.Init(audioFile);
            }
            TimeOffSong = audioFile is null ? TimeSpan.Zero : audioFile.TotalTime;
        }
        #endregion

        #region Public Methods

        public void PlayNow(Song song)
        {
            SongQueue.Clear();
            SongQueue.Enqueue(song);
            PlayNextSongFromQueue();
        }

        public void PlayNow(IEnumerable<Song> songs)
        {
            SongQueue.Clear();
            if (songs is null || songs.Any()) return;

            foreach (Song s in songs)
            {
                SongQueue.Enqueue(s);
            }

            PlayNextSongFromQueue();
        }

        public void AddToQueue(Song song)
        {
            SongQueue.Enqueue(song);

            if (outputDevice is null)
            {
                PlayNextSongFromQueue();
            }
        }

        public void AddToQueue(IEnumerable<Song> songs)
        {
            if (songs is null || songs.Any()) return;

            foreach (Song s in songs)
            {
                SongQueue.Enqueue(s);
            }

            if (outputDevice is null)
            {
                PlayNextSongFromQueue();
            }
        }

        public void ClearPlayback()
        {
            CleanPlayback();
        }

        #endregion

        #region Properties

        private Song? _selectedMusic;
        public Song? SongPlaying
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

        private Queue<Song> _songQueue;
        public Queue<Song> SongQueue
        {
            get
            {
                return _songQueue;
            }
            set
            {
                if (_songQueue != value)
                {
                    _songQueue = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
