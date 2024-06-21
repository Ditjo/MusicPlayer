using System;
using System.Collections.Generic;
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

namespace MusicPlayer.UI.Views.SongControls
{
    /// <summary>
    /// Interaction logic for SongControlsView.xaml
    /// </summary>
    public partial class SongControlsView : UserControl
    {
        public SongControlsView()
        {
            InitializeComponent();
        }

        //private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        //{
        //    if (sender is Slider slider)
        //    {
        //        var t = slider.Value;
        //        _viewModel.SetTime(TimeSpan.FromSeconds(t));
        //    }
        //    _viewModel.isDragging = false;
        //}

        //private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        //{
        //    _viewModel.isDragging = true;
        //}

    }
}
