using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Gallery
    {
        [Key] public int EventId { get; set; }
        public Event Event { get; set; }
        [StringLength(10)] public string Year { get; set; }
        [StringLength(50)] public string Name { get; set; }
        public string MainImg { get; set; }
        [Column(TypeName = "jsonb")] public List<GalleryImage> Images { get; set; }
    }

    public class GalleryImage
    {
        public Guid ImageId { get; set; } = Guid.NewGuid();
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<string> LikedByUserIds { get; set; }
        public List<GalleryImageComment> Comments { get; set; }
        public string Image { get; set; }
    }

    public class GalleryImageComment
    {
        public Guid CommentId { get; set; } = Guid.NewGuid();
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Comment { get; set; }
    }
}
