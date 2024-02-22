using MusicPlayer.UI.Base;
using MusicPlayer.Services.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.Services.Navigation
{
    public class NavigationCommandBase : ViewModelBase, IRequestForNavigationEvent
    {
        public event Action<string, object> RequestForNavigationEvent;
        public ICommand NavigationCommand { get; }

        public NavigationCommandBase()
        {
            NavigationCommand = new RelayCommand<object>(OnNavigateCommand);
        }

        private void OnNavigateCommand(object obj)
        {
            RequestForNavigation(obj.ToString());
        }

        internal void RequestForNavigation(string obj, object entity = null)
        {
            RequestForNavigationEvent(obj, entity);
        }

    }
}
