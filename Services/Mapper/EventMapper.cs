using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public class EventMapper
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventMapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventVm>> EventVmList(List<Event> events)
        {
            var eventVmList = new List<EventVm>();

            foreach (var eEvent in events)
            {
                var eventVm = await EventVm(eEvent);
                eventVmList.Add(eventVm);
            }

            return eventVmList;
        }

        public async Task<EventVm> EventVm(Event evnt)
        {
            var foundUsers = await _unitOfWork.User.GetAll(u => evnt.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");

            var teamVmList = await TeamMapper.TeamVmList(evnt.Teams);


            return new EventVm
            {
                EventId = evnt.EventId,
                Name = evnt.Name,
                Location = LocationVm(evnt.Location),
                Itineraries = ItineraryListVm(evnt.Itineraries),
                RegisteredUsers = UserMapper.RegisteredUserVmList(foundUsers),
                Teams = teamVmList,
                Year = evnt.Year,
                CreatedOn = evnt.CreatedOn,
                IsCurrentYear = evnt.IsCurrentYear,
                IsPublished = evnt.IsPublished,
                UpdatedOn = evnt.UpdatedOn
            };
        }

        public Location Location(LocationVm eventLocationVm)
        {
            return new Location
            {
                Name = eventLocationVm.Name,
                StreetNumbers = eventLocationVm.StreetNumbers,
                StreetName = eventLocationVm.StreetName,
                City = eventLocationVm.City,
                PostalCode = eventLocationVm.PostalCode
            };
        }

        public LocationVm LocationVm(Location location)
        {
            return new LocationVm
            {
                Name = location.Name,
                StreetNumbers = location.StreetNumbers,
                StreetName = location.StreetName,
                City = location.City,
                PostalCode = location.PostalCode
            };
        }

        public List<Itinerary> Itineraries(IEnumerable<ItineraryVm> itineraryVms)
        {
            var itineraries = new List<Itinerary>();
            foreach (var itineraryVm in itineraryVms)
            {
                var itinerary = Itinerary(itineraryVm);

                itineraries.Add(itinerary);
            }

            return itineraries;
        }

        public Itinerary Itinerary(ItineraryVm itineraryVm)
        {
            return new Itinerary
            {
                Day = itineraryVm.Day,
                Description = itineraryVm.Description,
                Time = itineraryVm.Time
            };
        }

        public List<ItineraryVm> ItineraryListVm(IEnumerable<Itinerary> itineraries)
        {
            var itineraryVmList = new List<ItineraryVm>();


            foreach (var itinerary in itineraries)
            {
                var itineraryVm = ItineraryVm(itinerary);

                itineraryVmList.Add(itineraryVm);
            }


            return itineraryVmList;
        }


        public ItineraryVm ItineraryVm(Itinerary itinerary)
        {
            return new ItineraryVm
            {
                Day = itinerary.Day,
                Description = itinerary.Description,
                Time = itinerary.Time
            };
        }

        public async Task<EventResultVm> EventResultsVm(EventResults eventResults)
        {
            var teamVmList = await TeamMapper.TeamVmList(eventResults.Teams);
            var scrambleChampVmList = await UserMapper.ScrambleChampVms(eventResults.ScrambleChamps);
            return new EventResultVm
            {
                EventId = eventResults.EventId,
                Teams = teamVmList,
                ScrambleChamps = scrambleChampVmList,
                IsActive = eventResults.IsActive
            };
        }
    }
}
