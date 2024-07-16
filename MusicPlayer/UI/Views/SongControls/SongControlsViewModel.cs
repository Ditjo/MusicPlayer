using MusicPlayer.Data.Models;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Base;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MusicPlayer.UI.Views.SongControls
{
    public class SongControlsViewModel : ViewModelBase
    {
        private MusicPlayerState _state;

        private bool musicNotStoppedByPerson = true;
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        private readonly DispatcherTimer musicTimer;

        public ICommand NavigationCommand { get; set; }
        public ICommand PlayMusicCommand { get; set; }
        public ICommand RewindMusicCommand { get; set; }
        public ICommand SkipTimeInMusicCommand { get; set; }
        public ICommand PreviousSongCommand { get; set; }
        public ICommand NextSongCommand { get; set; }

        public SongControlsViewModel(MusicPlayerState state)
        {
            _state = state;

            PlayMusicCommand = new RelayCommand(PlayMusic, CanPlayMusic);
            RewindMusicCommand = new RelayCommand(OnRewindMusicCommand);
            SkipTimeInMusicCommand = new RelayCommand<object>(OnSkipTimeInMusicCommand);
            PreviousSongCommand = new RelayCommand(OnPreviousSongCommand, CanPreviousSongCommand);
            NextSongCommand = new RelayCommand(OnNextSongCommand, CanNextSongCommand);

            musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromMilliseconds(1000);
            musicTimer.Tick += TimerTick;

            InitState();

            OutputVolume = 100;
            PlayBtn = "Play";
            
        }

        private void InitState()
        {
            if (_state != null)
            {
                CurrentSong = _state.CurrentSong;
                SongQueue = _state.SongQueue ?? new Queue<Song>();
                PastSongs = _state.PastSong ?? new Stack<Song>();
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

        #region Commands

        private void PlayMusic()
        {
            Debug.WriteLine("Playing Music...");
            if (CurrentSong != null && outputDevice == null) InitSongToPlay(CurrentSong);
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
            return CurrentSong is not null && System.IO.File.Exists(CurrentSong.Url);
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

        private void OnPreviousSongCommand()
        {
            //TODO: If Playlist is done playing, you wont be able to go back. Change behavior?
            if(audioFile == null) return;

            if(audioFile.CurrentTime > new TimeSpan(0,0,5) || PastSongs.Count == 0)
            {
                audioFile.Position = 0;
            }
            else
            {
                SongQueue = ReturnPreviousSongToQueue(SongQueue, PastSongs.Pop());
                SetUpSongToPlay();
            }
        }

        private bool CanPreviousSongCommand()
        {
            return true;
        }

        private void OnNextSongCommand()
        {
            SongQueue = RemoveFirstInLineSongFromQueue(SongQueue);
            PlayNextSongFromQueue();
        }

        private bool CanNextSongCommand()
        {
            return true;
        }

        #endregion

        #region SongQueueControls

        //Below Commands used to set song and start next song.

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (musicNotStoppedByPerson)
            {
                SongQueue = RemoveFirstInLineSongFromQueue(SongQueue);
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
            if(CurrentSong is not null) 
                PastSongs.Push(CurrentSong);
            SetUpSongToPlay();
        }

        private void SetUpSongToPlay()
        {
            CleanPlayback();
            if (SongQueue is null || SongQueue.Count == 0) return;

            CurrentSong = SongQueue.Peek();

            GetStateOfMusicPlayer();

            InitSongToPlay(CurrentSong);
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

        #region SaveStateOfMusicPlayer

        private void GetStateOfMusicPlayer()
        {
            _state.CurrentSong = CurrentSong;
            _state.SongQueue = SongQueue;
            _state.PastSong = PastSongs;

            SaveStateOfMusicPlayer(_state);
        }

        #endregion

        #region Public Methods

        public void PlayNow(Song song)
        {
            SongQueue.Clear();
            PastSongs.Clear();
            SongQueue = AddSongToQueue(SongQueue, song);
            PlayNextSongFromQueue();
        }

        public void PlayNow(IEnumerable<Song> songs)
        {
            SongQueue.Clear();
            PastSongs.Clear();
            if (songs is null || songs.Any()) return;

            foreach (Song song in songs)
            {
                SongQueue = AddSongToQueue(SongQueue, song);
            }

            PlayNextSongFromQueue();
        }

        public void AddToQueue(Song song)
        {
            SongQueue = AddSongToQueue(SongQueue, song);

            if (outputDevice is null)
            {
                PlayNextSongFromQueue();
            }
            else
            {
                GetStateOfMusicPlayer();
            }
        }

        public void AddToQueue(IEnumerable<Song> songs)
        {
            if (songs is null || songs.Any()) return;

            foreach (Song song in songs)
            {
                SongQueue = AddSongToQueue(SongQueue, song);
            }

            if (outputDevice is null)
            {
                PlayNextSongFromQueue();
            }
            else
            {
                GetStateOfMusicPlayer();
            }
        }

        public void ClearPlayback()
        {
            CleanPlayback();
        }

        #endregion

        #region WorkQueueMethods

        private static Queue<Song> AddSongToQueue(Queue<Song> queue, Song song)
        {
            Queue<Song> list = new();
            foreach (Song s in queue)
            {
                list.Enqueue(s);
            }
            list.Enqueue(song);
            return list;
        }

        private static Queue<Song> RemoveFirstInLineSongFromQueue(Queue<Song> queue)
        {
            queue.Dequeue();
            Queue<Song> list = new();
            foreach (Song song in queue)
            {
                list.Enqueue(song);
            }
            return list;
        }

        private static Queue<Song> ReturnPreviousSongToQueue(Queue<Song> queue, Song previousSong)
        {
            Queue<Song> list = new();
            list.Enqueue(previousSong);
            foreach (Song s in queue)
            {
                list.Enqueue(s);
            }
            return list;
        }

        private static Queue<Song> TakeNextSongInQueue()
        {
            return null;
        }

        #endregion

        #region Properties

        private Song? _currentSong;
        public Song? CurrentSong
        {
            get
            {
                return _currentSong;
            }
            set
            {
                if (_currentSong != value)
                {
                    _currentSong = value;
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

        private Stack<Song> _pastSongs;
        public Stack<Song> PastSongs
        {
            get
            {
                return _pastSongs;
            }
            set
            {
                if (_pastSongs != value)
                {
                    _pastSongs = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
