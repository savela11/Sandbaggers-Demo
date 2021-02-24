using System;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class EventVm
    {
        public int EventId { get; set; }

        public string Name { get; set; }
        public LocationVm Location { get; set; }
        public List<ItineraryVm> Itineraries { get; set; }
        public List<RegisteredUserVm> RegisteredUsers { get; set; }
        public List<TeamVm> Teams { get; set; }

        public string Year { get; set; }
        public bool IsCurrentYear { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class ItineraryVm
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
    }

    public class LocationVm
    {
        public string Name { get; set; }
        public string StreetNumbers { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }

    public class RegisteredUserVm
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
    }

    public class TeamVm
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public int EventId { get; set; }

        public TeamMemberVm Captain { get; set; }

        public List<TeamMemberVm> TeamMembers { get; set; }


    }

    public class TeamMemberVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
    }

public class AdminEventManagerVm
{

        public int EventId { get; set; }
        public string Name { get; set; }
        public LocationVm Location { get; set; }
        public List<ItineraryVm> Itineraries { get; set; }
        public List<RegisteredUserVm> RegisteredUsers { get; set; }
        public List<RegisteredUserVm> UnRegisteredUsers { get; set; }
        public List<TeamVm> Teams { get; set; }
        public string Year { get; set; }
        public bool IsCurrentYear { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
}
}

