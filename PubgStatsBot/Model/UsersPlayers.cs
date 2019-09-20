using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
    public class UsersPlayers
    {
        public int ID { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public  IEnumerable<Player> Players { get; set; }
    }
}
