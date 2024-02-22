using MusicPlayer.Services.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UI.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IRequestForNavigationEvent
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<string, object>? RequestForNavigationEvent;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
