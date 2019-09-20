namespace PubgSDK.Models
{
    public class Participant
    {
        public string Id { get; set; }
        public virtual ParticipantsStats Stats { get; set; }
        public long StatsID { get; set; }
    }

}
