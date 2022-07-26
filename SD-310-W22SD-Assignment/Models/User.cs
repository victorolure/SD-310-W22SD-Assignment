﻿using System;
using System.Collections.Generic;

namespace SD_310_W22SD_Assignment.Models
{
    public partial class User
    {
        public User()
        {
            Collections = new HashSet<Collection>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Wallet { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
        
    }
}
