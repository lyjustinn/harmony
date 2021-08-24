using System.Collections.Generic;

namespace Harmony.Models.User
{
    public class CurrentUser
    {
        public string Display_name { get; set; }

        public string Id { get; set; }

        public List<Image> Images { get; set; }
    }
}