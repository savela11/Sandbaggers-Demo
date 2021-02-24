using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{


    public class CreateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }

    public class AddOrRemoveUserFromRoleDto
    {
        [Required] public string RoleName { get; set; }
        [Required] public string UserId { get; set; }
    }
}
