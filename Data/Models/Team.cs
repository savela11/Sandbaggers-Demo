using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Data.Models
{
    public class Team
    {
        [Key] public int TeamId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Color { get; set; }
        [Required] public int EventId { get; set; }
        public Event Event { get; set; }
        public string CaptainId { get; set; }
        public List<string> TeamMemberIds { get; set; }
    }
}
