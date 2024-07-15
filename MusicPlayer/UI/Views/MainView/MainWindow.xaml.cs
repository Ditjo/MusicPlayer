using Microsoft.Win32;
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
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            OnStartUp();
            _viewModel = new MainViewModel();
            Closing += OnClosingEvent;
            DataContext = _viewModel;

        }

        public void OnStartUp()
        {

        }

        public void OnClosingEvent(object? sender, CancelEventArgs e)
        {
            Debug.WriteLine("Closing Program");
            _viewModel.SongControls.ClearPlayback();
        }
        
    }
}