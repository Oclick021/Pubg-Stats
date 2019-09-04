using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Interfaces
{
    public interface ISeasonStats :IStats
    {
        float BestRankPoint { get; set; }
        int DailyKills { get; set; }
        int DailyWins { get; set; }
        int Losses { get; set; }
        int MaxKillStreaks { get; set; }
        float RankPoints { get; set; }
        int RoundsPlayed { get; set; }
        int Top10s { get; set; }
        int WeeklyKills { get; set; }
        int WeeklyWins { get; set; }
        int Wins { get; set; }


        string GetKD();
        string GetWinRate();
        string GetTop10Rate();
    }
}
