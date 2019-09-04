namespace PubgSDK.Models
{
    public class Participant
    {
        public string Id { get; set; }
        public virtual ParticipantsStats Stats { get; set; }
        public string Actor { get; set; }
        public string ShardId { get; set; }
    }

}
