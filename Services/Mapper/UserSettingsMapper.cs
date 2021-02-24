using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class UserSettingsMapper
    {
        public static UserSettingsVm UserSettingsVm(UserSettings userSettings)
        {
            return new UserSettingsVm
            {
                FavoriteLinks = FavoriteLinkVms(userSettings.FavoriteLinks.AsEnumerable()),
                IsContactNumberShowing = userSettings.IsContactEmailShowing,
                IsContactEmailShowing = userSettings.IsContactEmailShowing
            };
        }

        public static List<FavoriteLink> FavoriteLinks(IEnumerable<FavoriteLinkVm> favoriteVmLinks)
        {
            var favoriteLinks = new List<FavoriteLink>();
            foreach (var favoriteVmLink in favoriteVmLinks)
            {
                var favoriteLink = new FavoriteLink
                {
                    Name = favoriteVmLink.Name,
                    Link = favoriteVmLink.Link
                };

                favoriteLinks.Add(favoriteLink);
            }

            return favoriteLinks;
        }

        public static List<FavoriteLinkVm> FavoriteLinkVms(IEnumerable<FavoriteLink> favoriteLinks)
        {
            var favoriteLinkVmList = new List<FavoriteLinkVm>();

            foreach (var favoriteLink in favoriteLinks)
            {
                var favoriteLinkVm = FavoriteLinkVm(favoriteLink);

                favoriteLinkVmList.Add(favoriteLinkVm);
            }

            return favoriteLinkVmList;
        }


        public static FavoriteLinkVm FavoriteLinkVm(FavoriteLink favoriteLink)
        {
            return new FavoriteLinkVm
            {
                Name = favoriteLink.Name,
                Link = favoriteLink.Link
            };
        }
    }
}
