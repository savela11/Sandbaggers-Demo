using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Models
{
    public class Bet
    {
        [Key] public int BetId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required] public string CreatedByUserId { get; set; }


        public int CanAcceptNumber { get; set; }

        public List<string> AcceptedByUserIds { get; set; }

        [DataType(DataType.Date)] public DateTime CreatedOn { get; set; }

        [DataType(DataType.Date)] public DateTime UpdatedOn { get; set; }

        public bool IsActive { get; set; } = false;
        public bool DoesRequirePassCode { get; set; } = false;
    }
}
