using System.Collections.Generic;

namespace Models.ViewModels
{
    public class CourseVm
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public LocationVm Location { get; set; }
        public List<RoundVm> Rounds { get; set; }
    }
}