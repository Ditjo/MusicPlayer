using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Data.Models
{
    public class MusicPlayerState
    {
        public Song? CurrentSong { get; set; }
        public Queue<Song> SongQueue { get; set; }
        public Stack<Song> PastSong { get; set; }

        public MusicPlayerState()
        {
            SongQueue = new Queue<Song>();
            PastSong = new Stack<Song>();
        }
    }
}
