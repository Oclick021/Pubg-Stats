using PubgSDK.Interfaces;

namespace PubgSDK.Models
{
    public class ParticipantsStats : IParticipantsStats
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PlayerId { get; set; }
        public int DBNOs { get; set; }
        public int Boosts { get; set; }
        public string DeathType { get; set; }
        public int Heals { get; set; }
        public int KillPlace { get; set; }
        public int KillPointsDelta { get; set; }
        public int KillStreaks { get; set; }
        public int LastKillPoints { get; set; }
        public int LastWinPoints { get; set; }
        public int MostDamage { get; set; }
        public int Revives { get; set; }
        public float RideDistance { get; set; }
        public int RoadKills { get; set; }
        public float SwimDistance { get; set; }
        public int TeamKills { get; set; }
        public float TimeSurvived { get; set; }
        public int VehicleDestroys { get; set; }
        public float WalkDistance { get; set; }
        public int WeaponsAcquired { get; set; }
        public int WinPlace { get; set; }
        public int WinPointsDelta { get; set; }
        public int Assists { get; set; }
        public float DamageDealt { get; set; }
        public int HeadshotKills { get; set; }
        public int Kills { get; set; }
        public float LongestKill { get; set; }
 

        public string GetListValue()
        {
            throw new System.NotImplementedException();
        }

        public static string GetTitles()
        {
            throw new System.NotImplementedException();
        }
    }



}
