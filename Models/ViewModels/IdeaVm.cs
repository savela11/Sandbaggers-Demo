using System;

namespace Models.ViewModels
{
    public class IdeaVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CreatedByUserVm CreatedBy { get; set; }

        // private DateTime _dateValue;

        public string CreatedOn { get; set; }
        // public string CreatedOn
        // {
        //     // get { return _dateValue.ToString("MM/dd/yyyy"); }
        //     get { return _dateValue.ToString("F"); }
        //     set { DateTime.TryParse(value, out _dateValue); }
        //     // set { DateTime.Parse(value); }
        // }

        public string UpdatedOn { get; set; }
    }
}
