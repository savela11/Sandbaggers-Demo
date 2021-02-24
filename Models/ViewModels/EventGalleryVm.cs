using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class EventGalleryVm
    {
        public int EventId { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string MainImg { get; set; }

        public List<GalleryImgVm> Images { get; set; }
    }

    public class GalleryWithNoImagesVm
    {
        public int EventId { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string MainImg { get; set; }
        public int NumOfImages { get; set; }
    }

    public class GalleryImgVm
    {
        [Required] public Guid ImageId { get; set; }
        [Required] public ImageCreatedByUserVm CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<LikeImageVm> Likes { get; set; }
        public List<CommentImgVm> Comments { get; set; }
        [Required] public string Image { get; set; }
    }

    public class LikeImageVm
    {
        [Required] public string FullName { get; set; }
        [Required] public string UserId { get; set; }
    }

    public class CommentImgVm
    {
        [Required] public Guid CommentId { get; set; }
        [Required] public CommentCreatedByUserVm CreatedByUser { get; set; }
        [Required] public DateTime CreatedOn { get; set; }
        [Required] public string Comment { get; set; }
    }


    public class ImageCreatedByUserVm
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
    }

    public class CommentCreatedByUserVm
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
    }
}
