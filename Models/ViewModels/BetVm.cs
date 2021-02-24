using System;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class BetVm
    {
        public int BetId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public CreatedByUserVm CreatedBy { get; set; }
        public int CanAcceptNumber { get; set; }
        public List<AcceptedByUserVm> AcceptedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool DoesRequirePassCode { get; set; }
    }
}
