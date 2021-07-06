using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Course
    {
        [Key] public int CourseId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "jsonb")] public Location Location { get; set; }
        public List<Round> Rounds { get; set; }
    }
}