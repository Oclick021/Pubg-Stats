using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
    public class Player
    {
        public int ID { get; set; }
        public string PubgID { get; set; }
        public DateTime? LastMatchNotified { get; set; }
        public List<Match> Matches { get; set; }

    }
}
