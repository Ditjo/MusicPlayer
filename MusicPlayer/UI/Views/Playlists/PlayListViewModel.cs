using MusicPlayer.Data.Repositories;
using MusicPlayer.Services.Command;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.UI.Views.Playlists
{
    public class PlayListViewModel : ViewModelBase
    {
        FileWorker fileWorker = new();
        public ICommand LoadDataCommand { get; set; }

        public PlayListViewModel()
        {
            LoadDataCommand = new RelayCommand(LoadData);
        }


        #region Commands

        public void LoadData()
        {
            FilePaths = fileWorker.ReadfilePaths();
        }

        #endregion

        #region Properties
        private List<string> _filePaths;
        public List<string> FilePaths
        {
            get 
            {
                return _filePaths; 
            }
            set 
            { 
                if (_filePaths != value)
                { 
                    _filePaths = value; 
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
