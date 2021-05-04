using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Views
{
    public class DraftManagerViewData
    {
        [Required] public int DraftId { get; set; }
        public List<RegisteredUserVm> RegisteredUsers { get; set; }
        public List<DraftUserVm> DraftUsers { get; set; }
        public List<DraftCaptainVm> DraftCaptains { get; set; }
        public bool IsDraftLive { get; set; }
    }
}
