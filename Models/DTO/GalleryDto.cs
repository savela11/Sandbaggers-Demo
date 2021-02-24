using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{


    public  class AddGalleryImgDto
    {
        [Required] public int EventId { get; set; }
        [Required] public string CreatedByUserId { get; set; }
        [Required] public string Image { get; set; }
    }

    public  class AddImageListToGalleryDto
    {
        [Required] public int EventId { get; set; }
        [Required] public string CreatedByUserId { get; set; }
        [Required] public List<string> Images { get; set; }
    }
}
