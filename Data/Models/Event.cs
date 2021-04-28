using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;


namespace Data.Models
{
    // TODO need to add a field for how many members allowed for event
    public class Event
    {
        [Key] public int EventId { get; set; }

        [StringLength(50)] public string Name { get; set; }

        // [Column(TypeName = "jsonb")] public Location Location { get; set; }
        public string Location { get; set; }


        public List<string> RegisteredUserIds { get; set; } = new List<string>();
        // [Column(TypeName = "jsonb")] public List<Itinerary> Itineraries { get; set; }

        //JSON VALUE CONVERTER
        public string Itineraries { get; set; } = "";
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

    public class Location
    {
        public string Name { get; set; }
        public string StreetNumbers { get; set; }
        public string StreetName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }

    // public class Location : ValueObject
    // {
    //     public string Name { get; set; }
    //     public string StreetNumbers { get; set; }
    //     public string StreetName { get; set; }
    //     public string State { get; set; }
    //     public string City { get; set; }
    //     public string PostalCode { get; set; }
    //
    //     private Location()
    //     {
    //     }
    //
    //     public Location(string name, string streetNumbers, string streetName, string state, string city, string postalCode)
    //     {
    //         Name = name;
    //         StreetNumbers = streetNumbers;
    //         StreetName = streetName;
    //         State = state;
    //         City = city;
    //         PostalCode = postalCode;
    //     }
    //
    //
    //     public override string ToString()
    //     {
    //         return $"{StreetNumbers} {StreetName}, {City}, {State} {PostalCode}";
    //     }
    //
    //     protected override IEnumerable<object> GetAtomicValues()
    //     {
    //         // Using a yield return statement to return each element one at a time
    //         yield return Name;
    //         yield return StreetNumbers;
    //         yield return StreetName;
    //         yield return State;
    //         yield return City;
    //         yield return PostalCode;
    //     }
    // }


    public class Itinerary
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
    }
}
