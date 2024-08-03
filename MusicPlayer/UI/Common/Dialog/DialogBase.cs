using MusicPlayer.Data.Models;
using MusicPlayer.Services.Command;
using MusicPlayer.Services.Helpers;
using MusicPlayer.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MusicPlayer.UI.Common.Dialog
{
    public class DialogBase : ViewModelBase
    {
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DialogBase() 
        {
            ConfirmCommand = new RelayCommand<object>(OnConfirmCommand, CanConfirmCommand);
            CancelCommand = new RelayCommand<object>(OnCancelCommand);

        }

        public virtual void OnConfirmCommand(object obj)
        {

        }
        public virtual bool CanConfirmCommand(object obj)
        {
            return true;
        }

        public virtual void OnCancelCommand(object obj)
        {

        }

    }
}
