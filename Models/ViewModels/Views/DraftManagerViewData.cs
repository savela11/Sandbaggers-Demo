using System.Collections.Generic;

namespace Models.ViewModels.Views
{
    public class DraftManagerViewData
    {
        public List<RegisteredUserVm> RegisteredUsers { get; set; }
        public List<TeamVm> Teams { get; set; }
    }
}
