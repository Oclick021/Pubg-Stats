using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Interfaces
{
    public interface IParticipantsStats : IStats
    {
        string Name { get; set; }
        string PlayerId { get; set; }
        int DBNOs { get; set; }
        int Boosts { get; set; }
        string DeathType { get; set; }
        int Heals { get; set; }
        int KillPlace { get; set; }
        int KillPointsDelta { get; set; }
        int KillStreaks { get; set; }
        int LastKillPoints { get; set; }
        int LastWinPoints { get; set; }
        int MostDamage { get; set; }
        int Revives { get; set; }
        float RideDistance { get; set; }
        int RoadKills { get; set; }
        float SwimDistance { get; set; }
        int TeamKills { get; set; }
        float TimeSurvived { get; set; }
        int VehicleDestroys { get; set; }
        float WalkDistance { get; set; }
        int WeaponsAcquired { get; set; }
        int WinPlace { get; set; }
        int WinPointsDelta { get; set; }
    }
}
