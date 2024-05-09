using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Data.Models
{
    class Song
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string SongName { get; set; }
        public string Artist {  get; set; }
        public DateTime Length { get; set; }
    }
}
