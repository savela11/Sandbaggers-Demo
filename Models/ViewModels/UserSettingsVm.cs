using System.Collections.Generic;

namespace Models.ViewModels
{
    public class UserSettingsVm
    {
        public List<FavoriteLinkVm> FavoriteLinks { get; set; }
        public bool IsContactNumberShowing { get; set; }
        public bool IsContactEmailShowing { get; set; }
    }


    public class FavoriteLinkVm
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
