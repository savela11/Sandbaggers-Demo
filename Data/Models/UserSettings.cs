using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class UserSettings
    {
        [Key] [Required] public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Column(TypeName = "jsonb")] public List<FavoriteLink> FavoriteLinks { get; set; } = new List<FavoriteLink>();

        public bool IsContactNumberShowing { get; set; } = false;

        public bool IsContactEmailShowing { get; set; } = false;
    }


    public class FavoriteLink
    {
        public string Name { get; set; }

        public string Link { get; set; }
    }
}
