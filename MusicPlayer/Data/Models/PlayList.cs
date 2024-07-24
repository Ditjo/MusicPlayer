using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Data.Models
{
    public class PlayList
    {
        public PlayList(string? title)
        {
            Title = title;
            Songs = new();
        }
        public int Id { get; set; }
        public string? Title { get; set; }
        public Queue<Song> Songs { get; set;}
    }
}
