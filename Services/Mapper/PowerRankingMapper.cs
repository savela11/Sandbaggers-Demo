using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class PowerRankingMapper
    {
        public static IUnitOfWork OfWork { get; }

        public static async Task<PowerRankingVm> PowerRankingVm(Event evnt)
        {
            var registeredUsers = await OfWork.User.GetAll(u => evnt.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");


            var registeredUsersVmList = UserMapper.RegisteredUserVmList(registeredUsers);

            var rankingVmList = new List<RankingVm>();
            foreach (var r in evnt.PowerRanking.Rankings)
            {
                var foundUser = registeredUsersVmList.FirstOrDefault(u => u.Id == r.UserId);
                if (foundUser == null) continue;
                var rankingVm = new RankingVm
                {
                    Handicap = r.Handicap,
                    Rank = r.Rank,
                    Trending = r.Trending,
                    Writeup = r.Writeup,
                    CreatedOn = r.CreatedOn,
                    FullName = foundUser.FullName,
                    UserId = foundUser.Id,
                    RankingId = r.RankingId,
                    UpdatedOn = r.UpdatedOn
                };
                rankingVmList.Add(rankingVm);
            }


            var powerRankingVm = new PowerRankingVm
            {
                EventId = evnt.EventId,
                Year = evnt.Year,
                Disclaimer = evnt.PowerRanking.Year,
                Rankings = rankingVmList,
                RegisteredUsers = registeredUsersVmList,
                CreatedOn = evnt.PowerRanking.CreatedOn,
                EventName = evnt.Name
            };
            return powerRankingVm;
        }

        public static async Task<RankingVm> RankingVm(Ranking ranking)
        {
            var foundUser = await OfWork.User.GetFirstOrDefault(u => u.Id == ranking.UserId);
            return new RankingVm
            {
                Handicap = ranking.Handicap,
                Rank = ranking.Rank,
                Trending = ranking.Trending,
                Writeup = ranking.Writeup,
                CreatedOn = ranking.CreatedOn,
                FullName = foundUser.FullName,
                UserId = foundUser.Id,
                RankingId = ranking.RankingId,
                UpdatedOn = ranking.UpdatedOn
            };
        }
    }
}
