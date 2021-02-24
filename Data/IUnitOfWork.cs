using System;
using System.Threading.Tasks;
using Data.Repository.Interface;

namespace Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserManagerRepo UserManager { get; }
        ISignInManagerRepo SignInManager { get; }
        IRoleManagerRepo RoleManager { get; }
        IUserRepo User { get; }
        IUserProfileRepo UserProfile { get; }
        IBetRepo Bet { get; }
        IEventResultsRepo EventResults { get; }
        IGalleryRepo Gallery { get; }
        IEventRepo Event { get; }
        IPowerRankingRepo PowerRanking { get; }
        ITeamRepo Team { get; }
        IIdeaRepo Idea { get; }



        Task Save();
    }
}
