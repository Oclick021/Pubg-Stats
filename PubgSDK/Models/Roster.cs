using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Models
{

    public class Roster
    {
        public string Id { get; set; }
        public bool Won { get; set; }
        public virtual IEnumerable<Participant> Participants { get; set; }
    }
}
