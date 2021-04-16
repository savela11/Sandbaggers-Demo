using System;

namespace Models.ViewModels
{
    public class IdeaVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CreatedByUserVm CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
