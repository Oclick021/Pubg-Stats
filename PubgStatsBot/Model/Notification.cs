using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
   public class Notification
    {
        public int ID { get; set; }
        public ulong UserId { get; set; }
        public string MatchID { get; set; }
    }
}
