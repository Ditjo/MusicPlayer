using Microsoft.Win32;
using MusicPlayer.UI.Views.MainView;
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
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;


            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //    mediaPlayer.Open(new Uri(openFileDialog.FileName));

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    if (mediaPlayer.Source != null)
        //        lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        //    else
        //        lblStatus.Content = "No file selected...";
        //}

        //private void btnPlay_Click(object sender, RoutedEventArgs e)
        //{
        //    mediaPlayer.Play();
        //}

        //private void btnPause_Click(object sender, RoutedEventArgs e)
        //{
        //    mediaPlayer.Pause();
        //}

        //private void btnStop_Click(object sender, RoutedEventArgs e)
        //{
        //    mediaPlayer.Stop();
        //}
    }
}