﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public string UserTag { get; set; }


    }
}
