using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<ServiceResponse<List<EventVm>>> Events();

        Task<ServiceResponse<EventVm>> CreateEvent(CreateEventDto createEventDto);

        Task<ServiceResponse<EventVm>> GetEventById(int id);


        Task<ServiceResponse<EventVm>> UpdateEvent(EventVm sandbaggerEventVm);

        Task<ServiceResponse<RegisteredUserVm>> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto);
        Task<ServiceResponse<string>> RemoveUserFromEvent(RemoveUserFromEventDto removeUserFromEventDto);


        Task<ServiceResponse<List<EventVm>>> PublishedEventsByYear();


        Task<ServiceResponse<bool>> DeleteEvent(int eventId);
        Task<ServiceResponse<AdminEventManagerVm>> EventForEventManager(int eventId);
    }
}
