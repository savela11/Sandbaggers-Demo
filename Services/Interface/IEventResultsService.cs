using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IEventResultsService
    {
        Task<ServiceResponse<List<ScrambleChampVm>>> ScrambleChamps();
        Task<ServiceResponse<EventResultVm>> EventResults(int id);

    }
}
