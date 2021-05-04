using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IEventManagerService
    {
        Task<ServiceResponse<int>> CreateEvent(CreateEventDto createEventDto);
        Task<ServiceResponse<AdminEventManagerVm>> EventForEventManager(int eventId);
        Task<ServiceResponse<RegisteredUserVm>> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto);
        Task<ServiceResponse<string>> RemoveUserFromEvent(RemoveUserFromEventDto removeUserFromEventDto);
    }
}
