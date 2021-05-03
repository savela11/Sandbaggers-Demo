using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ViewModels
{
    public class DraftUserVm
    {
        [Required] public string Id { get; set; }
        [Required] public string FullName { get; set; }
        [Column(TypeName = "decimal(10,1)")] public decimal BidAmount { get; set; } = 0;

    }
       public class DraftCaptainVm
        {
            [Required] public string Id { get; set; }
            [Required] public string FullName { get; set; }
            [Required] public string TeamName { get; set; }
            [Column(TypeName = "decimal(10,1)")] public decimal Balance { get; set; } = 0;

        }
}
