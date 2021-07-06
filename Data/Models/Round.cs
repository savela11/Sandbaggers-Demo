using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Round
    {
        [Key] [Required] public int RoundId { get; set; }
        [Required] public int CourseId { get; set; }
        [Required] public ApplicationUser User { get; set; }
        [Column(TypeName = "jsonb")] public List<Guest> Guests { get; set; } = new List<Guest>();
    }


    public class Guest
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}