using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;
using Models.ViewModels.Views;
using Utilities;

namespace Services.Interface
{
    public interface IDashboardService
    {
        Task<ServiceResponse<DashboardViewData>> DashboardData();

        Task<ServiceResponse<List<BetVm>>> ActiveBets();
    }
}
