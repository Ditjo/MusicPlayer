using MusicPlayer.UI.Common.Dialog;
using MusicPlayer.UI.Views.NewPlaylist;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.UI.Views.Playlists
{
    /// <summary>
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        public PlaylistView()
        {
            InitializeComponent();
        }

        //private void NewPlaylistDialogOpen(object sender, RoutedEventArgs e)
        //{
        //    DialogWindow dialogWindow = new DialogWindow();
        //    dialogWindow.Title = "New Playlist";
        //    dialogWindow.DialogContent.Content = new NewPlaylistViewModel();
        //    dialogWindow.Owner = Application.Current.MainWindow;
        //    dialogWindow.DataContext = Application.Current.MainWindow.DataContext;
        //    dialogWindow.ShowDialog();
        //}
    }
}
