using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PubgSDK.Models
{

    public class Match
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public int Duration { get; set; }
        public virtual ICollection<Roster> Rosters { get; set; }
        public string GameMode { get; set; }
        public string MapName { get; set; }
        public bool IsCustomMatch { get; set; }
        public string SeasonState { get; set; }

        public virtual ICollection<PlayerMatch> Players { get; set; }
        public Match()
        {

        }
     
    }



   

}
