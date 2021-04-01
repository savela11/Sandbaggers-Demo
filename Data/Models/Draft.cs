using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Draft
    {
        [Required] public int Id { get; set; }
        [Required] public int EventId { get; set; }
        public Event Event { get; set; }

        [Column(TypeName = "jsonb")]
        public List<DraftUser> DraftUsers { get; set; }

        [Column(TypeName = "jsonb")]
        public List<DraftCaptain> DraftCaptains { get; set; }
    }


    public class DraftUser
    {
        [Required] public string Id { get; set; }
        [Required] public string FullName { get; set; }
        [Column(TypeName = "decimal(10,1)")] public decimal BidAmount { get; set; } = 0;

    }

    public class DraftCaptain
    {

        [Required] public string Id { get; set; }
        [Required] public string FullName { get; set; }
        [Column(TypeName = "decimal(10,1)")] public decimal Balance { get; set; } = 0;

    }
}
