using System.Collections.Generic;
using Harmony.Models.User;

namespace Harmony.Models.Song
{
    public class SongTrack
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public List<Artist> Artists { get; set; }
    }
}