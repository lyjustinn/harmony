using Harmony.Models.User;

namespace Harmony.Models.Song
{
    public class SongItem
    {
        public SongTrack Track { get; set; }

        public Contributor Added_by { get; set; }
    }
}