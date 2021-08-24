using System.Collections.Generic;
using Harmony.Models.User;

namespace Harmony.Models.Playlist
{
    public class PlaylistItem
    {
        public string Description { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public Contributor Owner { get; set; }

        public List<Image> Images { get; set; }

        public PlaylistTrack Tracks { get; set; }
    }
}