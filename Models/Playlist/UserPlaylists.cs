using System;
using System.Collections.Generic;

namespace Harmony.Models.Playlist 
{
    public class UserPlaylists
    {
        public string Uid { get; set; }
        
        public List<PlaylistItem> Items { get; set; }

        public int Total { get; set; }
    }
}