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
        //private MediaPlayer mediaPlayer = new MediaPlayer();
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            Closing += _viewModel.OnClosingCommand;
            DataContext = _viewModel;
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if(sender is Slider slider) 
            {
                var t = slider.Value;
                _viewModel.SetTime(TimeSpan.FromSeconds(t));
            }
            _viewModel.isDragging = false;
        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _viewModel.isDragging = true;
        }
    }
}