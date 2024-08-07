﻿using Microsoft.Win32;
using MusicPlayer.Data.Models;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Views.MainView;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string statePath = "state.json";
        private string playlistPath = "playlists.json";

        private MainViewModel _viewModel;
        private MusicPlayerState _state;
        private ListOfPlayLists _playlists;

        public MainWindow()
        {
            InitializeComponent();
            OnStartUp();
            _viewModel = new MainViewModel(_state, _playlists.PlayLists);
            Closing += OnClosingEvent;
            DataContext = _viewModel;
        }

        public void OnStartUp()
        {
            GetStateOfMusicplayer();
            GetPlaylists();
        }

        public void OnClosingEvent(object? sender, CancelEventArgs e)
        {
            Debug.WriteLine("Closing Program");
            _viewModel.SongControls.ClearPlayback();
            //FileHandler.SaveToJSON<MusicPlayerState>(_state ,statePath);
        }

        private void GetStateOfMusicplayer()
        {
            var state = FileHandler.LoadFromJSON<MusicPlayerState>(statePath);
            _state = state is null ? new MusicPlayerState() : state;
        }

        private void GetPlaylists()
        {
            var playlists = FileHandler.LoadFromJSON<ListOfPlayLists>(playlistPath);
            _playlists = playlists is null ? new ListOfPlayLists() : playlists;
        }
    }
}