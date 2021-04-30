using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Models
{
    // TODO need to add a field for how many members allowed for event
    public class Event
    {
        [Key] public int EventId { get; set; }

        [StringLength(50)] public string Name { get; set; }

        [Column(TypeName = "jsonb")] public Location Location { get; set; }
        public List<string> RegisteredUserIds { get; set; } = new List<string>();
        [Column(TypeName = "jsonb")] public List<Itinerary> Itineraries { get; set; }

        //JSON VALUE CONVERTER
        // public string Itineraries { get; set; } = "";
        public string Year { get; set; }

        public int NumOfTeams { get; set; } = 0;
        public int NumOfUsers { get; set; } = 0;
        public bool IsCurrentYear { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();
        public EventResults EventResults { get; set; }
        public Gallery Gallery { get; set; }
        public PowerRanking PowerRanking { get; set; }
        public Draft Draft { get; set; }
    }




}
