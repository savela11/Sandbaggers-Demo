using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class EventResults
    {
        [Key] public int EventId { get; set; }
        public Event Event { get; set; }
        public List<string> ScrambleChamps { get; set; }
        public List<string> Teams { get; set; }
        public bool IsActive { get; set; }
    }
}
