using Data;
using Services.Interface;

namespace Services
{
    public class Service : IService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            User = new UserService(_unitOfWork);
            Bet = new BetService(_unitOfWork);
            Dashboard = new DashboardService(_unitOfWork);
            Event = new EventService(_unitOfWork);
            Contact = new ContactService(_unitOfWork);
            Gallery = new GalleryService(_unitOfWork);
            Idea = new IdeaService(_unitOfWork);
            PowerRanking = new PowerRankingService(_unitOfWork);
            EventResult = new EventResultsService(_unitOfWork);
            Team = new TeamService(_unitOfWork);
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
    }
}
