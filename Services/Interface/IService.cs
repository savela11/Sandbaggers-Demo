namespace Services.Interface
{
    public interface IService
    {
        public IUserService User { get; }
        public IBetService Bet { get; }
        public IDashboardService Dashboard { get; }
        public IEventService Event { get; }
        public IContactService Contact { get; }
        public IGalleryService Gallery { get; }
        public IIdeaService Idea { get; }
        public IPowerRankingService PowerRanking { get; }
        public IEventResultsService EventResult { get; }
    }
}
