using System.Collections.Generic;
using Harmony.Models.Song;

namespace Harmony.Models.Playlist
{
    public class PlaylistTrack
    {
        public string Href { get; set; }

        public List<SongItem> Items { get; set; }

        public int Total { get; set; }
    }
}