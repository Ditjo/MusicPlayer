using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Data.Models
{
    public class ListOfPlayLists
    {
        public ListOfPlayLists()
        {
            PlayLists = new();
        }
        public List<PlayList> PlayLists { get; set; }
    }
}
