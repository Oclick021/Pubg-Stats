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
        public int GetXP()
        {
            int xp = 0;
            xp += Assists * 5;
            xp += (int)DamageDealt;
            xp += DBNOs * 5;
            xp += HeadshotKills * 5;
            xp += Kills * 10;
            if (Kills > 1)
            {
                xp += KillStreaks * 15;
            }
            xp += Revives * 10;
            xp -= TeamKills * 10;
            var TimeSurviveds = ((TimeSurvived / 60) * 3);
            xp += (int)TimeSurviveds;
            xp += KillStreaks * 10;

            if (WinPlace == 1)
            {
                xp += 50;
            }
            else if (WinPlace == 2)
            {
                xp += 25;
            }
            else if (WinPlace == 3)
            {
                xp += 10;
            }

            return xp;
        }
    }



}
