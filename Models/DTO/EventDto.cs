using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{


    public class CreateEventDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Year { get; set; }

        public bool IsCurrentYear { get; set; } = false;

        public bool IsPublished { get; set; } = false;

        public DateTime CreatedOn { get; set; }
    }



    public class RegisterUserForEventDto
    {
        public int EventId { get; set; }
        public string UserId { get; set; }

    }

    public class RemoveUserFromEventDto
    {
        public int EventId { get; set; }
        public string UserId { get; set; }

    }
}
