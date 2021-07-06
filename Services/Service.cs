using Data;
using Services.Interface;

namespace Services
{
    public class Service : IService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _dbContext;

        public Service(IUnitOfWork unitOfWork, AppDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;

            User = new UserService(_dbContext);
            Bet = new BetService(_unitOfWork);
            Dashboard = new DashboardService(_unitOfWork);
            Event = new EventService(_unitOfWork);
            Contact = new ContactService(_unitOfWork);
            Gallery = new GalleryService(_unitOfWork);
            Idea = new IdeaService(_dbContext);
            PowerRanking = new PowerRankingService(_unitOfWork);
            EventResult = new EventResultsService(_unitOfWork);
            Team = new TeamService(_unitOfWork, _dbContext);
            Draft = new DraftService(_dbContext);
            DraftManager = new DraftManagerService(_dbContext);
            EventManager = new EventManagerService(_dbContext);
            TeamManager = new TeamManagerService(_dbContext);
            CourseManager = new CourseManagerService(_dbContext);
        }


        public IUserService User { get; private set; }
        public IBetService Bet { get; private set; }
        public IDashboardService Dashboard { get; private set; }
        public IEventService Event { get; private set; }
        public IContactService Contact { get; private set; }
        public IGalleryService Gallery { get; private set; }
        public IIdeaService Idea { get; private set; }
        public IPowerRankingService PowerRanking { get; private set; }
        public IEventResultsService EventResult { get; private set; }
        public ITeamService Team { get; private set; }
        public IDraftService Draft { get; private set; }
        public IDraftManagerService DraftManager { get; private set; }
        public IEventManagerService EventManager { get; private set; }
        public ITeamManagerService TeamManager { get; private set; }
           public ICourseManagerService CourseManager { get; private set; }
    }
}