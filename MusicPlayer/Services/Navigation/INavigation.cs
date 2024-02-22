using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Services.Navigation
{
    public interface IRequestForNavigationEvent
    {
        event Action<string, object> RequestForNavigationEvent;
    }
}
