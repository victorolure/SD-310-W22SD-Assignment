using System;
using System.Collections.Generic;

namespace SD_310_W22SD_Assignment.Models
{
    public partial class Song
    {
        public Song()
        {
            Collections = new HashSet<Collection>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Artist { get; set; }
        public int Price { get; set; }

        public virtual Artist ArtistNavigation { get; set; } = null!;
        public virtual ICollection<Collection> Collections { get; set; }
    }
}
