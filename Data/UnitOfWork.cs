using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UnitOfWork(AppDbContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;


            SignInManager = new SignInManagerRepo(_signInManager);
            UserManager = new UserManagerRepo(_userManager);
            RoleManager = new RoleManagerRepo(_roleManager);


            User = new UserRepo(_dbContext);
            UserProfile = new UserProfileRepo(_dbContext);
            Bet = new BetRepo(_dbContext);
            EventResults = new EventResultsRepo(_dbContext);
            Gallery = new GalleryRepo(_dbContext);
            Event = new EventRepo(_dbContext);
            PowerRanking = new PowerRankingRepo(_dbContext);
            Team = new TeamRepo(_dbContext);
            Idea = new IdeaRepo(_dbContext);
        }


        public IUserManagerRepo UserManager { get; private set; }
        public ISignInManagerRepo SignInManager { get; private set; }
        public IRoleManagerRepo RoleManager { get; private set; }
        public IUserRepo User { get; private set; }
        public IUserProfileRepo UserProfile { get; private set; }
        public IBetRepo Bet { get; private set; }
        public IEventResultsRepo EventResults { get; private set; }
        public IGalleryRepo Gallery { get; private set; }
        public IEventRepo Event { get; private set; }
        public IPowerRankingRepo PowerRanking { get; private set; }
        public ITeamRepo Team { get; private set; }

        public IIdeaRepo Idea { get; private set; }

        public void Dispose()
        {
            _dbContext.DisposeAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
