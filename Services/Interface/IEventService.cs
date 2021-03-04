using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<ServiceResponse<EventVm>> EventVm(Event evnt);
        Task<ServiceResponse<List<EventVm>>> EventVmList(List<Event> evnts);
        Task<ServiceResponse<List<Event>>> Events();

        Task<ServiceResponse<Event>> CreateEvent(CreateEventDto createEventDto);

        Task<ServiceResponse<Event>> GetEventById(int id);


        Task<ServiceResponse<Event>> UpdateEvent(EventVm sandbaggerEventVm);

        Task<ServiceResponse<RegisteredUserVm>> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto);
        Task<ServiceResponse<string>> RemoveUserFromEvent(RemoveUserFromEventDto removeUserFromEventDto);


        Task<ServiceResponse<List<Event>>> PublishedEventsByYear();


        Task<ServiceResponse<bool>> DeleteEvent(int eventId);
        Task<ServiceResponse<AdminEventManagerVm>> EventForEventManager(int eventId);
    }
}
