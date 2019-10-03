using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
    public class Log
    {
        public ulong ID { get; set; }

        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }

    }
}
