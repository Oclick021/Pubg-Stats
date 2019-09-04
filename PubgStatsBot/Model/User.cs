using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
    public class User
    {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public ICollection<Player> PlayerList { get; set; }
    }
}
