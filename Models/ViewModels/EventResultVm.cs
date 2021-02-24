using System.Collections.Generic;

namespace Models.ViewModels
{
    public class EventResultVm
    {
        public int EventId { get; set; }
        public List<TeamVm> Teams { get; set; }
        public List<ScrambleChampVm> ScrambleChamps { get; set; }
        public bool IsActive { get; set; }
    }

    public class ScrambleChampVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
    }
}
