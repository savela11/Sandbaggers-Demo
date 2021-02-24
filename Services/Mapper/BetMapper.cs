using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class BetMapper
    {
        public static IUnitOfWork _unitOfWork;

        public static async Task<BetVm> BetVm(Bet bet)
        {
            var createdByUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == bet.CreatedByUserId);

            var createdByUserVm = UserMapper.CreatedByUserVm(createdByUser);

            var acceptedByUsers = await _unitOfWork.User.GetAll(u => bet.AcceptedByUserIds.Contains(u.Id));

            var acceptedByUserVms = UserMapper.AcceptedByUserVmList(acceptedByUsers);


            return new BetVm
            {
                BetId = bet.BetId,
                Title = bet.Title,
                Description = bet.Description,
                Amount = bet.Amount,
                CreatedBy = createdByUserVm,
                CanAcceptNumber = bet.CanAcceptNumber,
                AcceptedBy = acceptedByUserVms,
                CreatedOn = bet.CreatedOn,
                UpdatedOn = bet.UpdatedOn,
                IsActive = bet.IsActive,
                DoesRequirePassCode = bet.DoesRequirePassCode
            };
        }


        public static async Task<List<BetVm>> BetVmList(IEnumerable<Bet> bets)
        {
            var betVmList = new List<BetVm>();

            foreach (var bet in bets)
            {
                var betVm = await BetVm(bet);


                betVmList.Add(betVm);
            }


            return betVmList;
        }
    }
}
