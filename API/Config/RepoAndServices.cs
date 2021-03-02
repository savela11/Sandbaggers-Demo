using Data;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interface;

namespace API.Config
{
    public static class RepoAndServices
    {
        public static void Repositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void Services(this IServiceCollection services)
        {
            services.AddTransient<IAzureStorageService, AzureStorageService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IBetService, BetService>();
            services.AddTransient<IIdeaService, IdeaService>();
            services.AddTransient<IEventResultsService, EventResultsService>();
            services.AddTransient<IGalleryService, GalleryService>();
            services.AddTransient<IPowerRankingService, PowerRankingService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<ITeamService, TeamService>();
        }
    }
}
