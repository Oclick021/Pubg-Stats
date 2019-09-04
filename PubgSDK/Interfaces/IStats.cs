using Pubg.Net.Models.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Interfaces
{
    public interface IStats
    {
        int Assists { get; set; }
        float DamageDealt { get; set; }
        int HeadshotKills { get; set; }
        int Kills { get; set; }
        float LongestKill { get; set; }

        string GetListValue();
    }
}
