using System.Collections.Generic;

namespace Harmony.Models {
    public class Album 
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public List<Image> Images { get; set; }
    }
}