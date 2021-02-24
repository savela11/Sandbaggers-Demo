using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{

    public class AddIdeaDto
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string UserId  { get; set; }
    }

    public class GetIdeaDto
    {
        public string UserId { get; set; }
        public int IdeaId { get; set; }
    }
}
