using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class BranchInformation
{
  
        [Key] public int Id { get; set; }
        public string HiddenValue { get; set; }
        public string BranchId { get; set; }
        public bool IsActive { get; set; } = false;

}