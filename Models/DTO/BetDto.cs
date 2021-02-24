using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{



    public class CreateBetDto
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }

        [DataType(DataType.Currency)] public decimal Amount { get; set; }

        [Required] public string UserId { get; set; }
        [Required] public int CanAcceptNumber { get; set; }


        public bool IsActive { get; set; } = false;
        public bool DoesRequirePassCode { get; set; } = false;
    }



    public class DeleteBetDto
    {
        [Required]
        public int betId { get; set; }
        [Required]
        public string userId { get; set; }
    }



    public class UserAcceptedBetDto
    {
        [Required] public int BetId { get; set; }
        [Required] public string UserId { get; set; }

    }
}
