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



        Task<ServiceResponse<Event>> GetEventById(int id);


        Task<ServiceResponse<Event>> UpdateEvent(EventVm sandbaggerEventVm);




        Task<ServiceResponse<List<Event>>> PublishedEventsByYear();



    }
}
