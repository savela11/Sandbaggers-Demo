using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
using Utilities;


namespace Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;


        private readonly EventMapper _eventMapper;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _eventMapper = new EventMapper(unitOfWork);
        }


        public async Task<ServiceResponse<EventVm>> CreateEvent(CreateEventDto createEventDto)
        {
            var serviceResponse = new ServiceResponse<EventVm>();
            try
            {
                // check if event by year or name exists
                var foundEventByYear = await _unitOfWork.Event.GetFirstOrDefault(e => e.Year == createEventDto.Year);
                if (foundEventByYear == null)
                {
                    var sandbaggerEvent = new Event
                    {
                        Name = createEventDto.Name,
                        Year = createEventDto.Year,
                        Location = new Location
                        {
                            Name = "",
                            City = "",
                            StreetName = "",
                            StreetNumbers = "",
                            PostalCode = ""
                        },
                        Itineraries = new List<Itinerary>(),
                        CreatedOn = DateTime.UtcNow
                    };

                    var createdEvent = await _unitOfWork.Event.AddAsync(sandbaggerEvent);
                    var eventResults = new EventResults
                    {
                        Event = createdEvent,
                        EventId = createdEvent.EventId,
                        Teams = new List<string>(),
                        ScrambleChamps = new List<string>(),
                        IsActive = false,
                    };
                    var eventGallery = new Gallery
                    {
                        EventId = createdEvent.EventId,
                        Event = createdEvent,
                        Name = createdEvent.Name,
                        Year = createdEvent.Year,
                        MainImg = "",
                        Images = new List<GalleryImage>()
                    };
                    await _unitOfWork.Gallery.AddNoReturn(eventGallery);
                    var powerRanking = new PowerRanking
                    {
                        Disclaimer = "",
                        CreatedOn = DateTime.UtcNow,
                        Rankings = new List<Ranking>(),
                        Year = createdEvent.Year,
                        EventId = createdEvent.EventId,
                        Event = createdEvent
                    };
                    await _unitOfWork.PowerRanking.AddNoReturn(powerRanking);

                    createdEvent.EventResults = eventResults;

                    await _unitOfWork.Save();


                    var eventVm = await _eventMapper.EventVm(createdEvent);


                    serviceResponse.Data = eventVm;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Event already created with the year " + createEventDto.Year;
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<EventVm>>> Events()
        {
            var serviceResponse = new ServiceResponse<List<EventVm>>();
            try
            {
                var foundEvents = await _unitOfWork.Event.GetAll(orderBy: e => e.OrderBy(d => d.Year), includeProperties: "Teams");

                var eventVmList = await _eventMapper.EventVmList(foundEvents.ToList());

                serviceResponse.Data = eventVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<EventVm>> GetEventById(int id)
        {
            var serviceResponse = new ServiceResponse<EventVm>();
            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == id, includeProperties: "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event found by Id";
                    return serviceResponse;
                }

                var eventVm = await _eventMapper.EventVm(foundEvent);


                serviceResponse.Data = eventVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<EventVm>> UpdateEvent(EventVm sandbaggerEventVm)
        {
            var serviceResponse = new ServiceResponse<EventVm>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == sandbaggerEventVm.EventId, includeProperties: "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found";
                    return serviceResponse;
                }

                foundEvent.Name = sandbaggerEventVm.Name;
                foundEvent.Itineraries = _eventMapper.Itineraries(sandbaggerEventVm.Itineraries);
                foundEvent.Location = _eventMapper.Location(sandbaggerEventVm.Location);
                foundEvent.UpdatedOn = DateTime.UtcNow;


                //check if there is already an event active or current year
                if (sandbaggerEventVm.IsPublished || sandbaggerEventVm.IsCurrentYear)
                {
                    var activeOrPublished = await _unitOfWork.Event.CheckPublishedOrActive(sandbaggerEventVm.EventId);

                    if (activeOrPublished)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "There is already an event that is active or set as current year.";
                        return serviceResponse;
                    }

                    foundEvent.IsPublished = sandbaggerEventVm.IsPublished;
                    foundEvent.IsCurrentYear = sandbaggerEventVm.IsCurrentYear;
                }
                else
                {
                    foundEvent.IsPublished = sandbaggerEventVm.IsPublished;
                    foundEvent.IsCurrentYear = sandbaggerEventVm.IsCurrentYear;
                }

                await _unitOfWork.Save();
                var eventVm = await _eventMapper.EventVm(foundEvent);
                serviceResponse.Data = eventVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<RegisteredUserVm>> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto)
        {
            var serviceResponse = new ServiceResponse<RegisteredUserVm>();
            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == registerUserForEventDto.UserId, includeProperties: "UserProfile");

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by ID";
                    return serviceResponse;
                }


                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == registerUserForEventDto.EventId, includeProperties: "Teams");

                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event or User found";
                    return serviceResponse;
                }


                // check if user exists in already registered
                if (foundEvent.RegisteredUserIds.Exists(id => id == foundUser.Id))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User already exists in event";
                    return serviceResponse;
                }

                if (foundEvent.RegisteredUserIds.Count >= 24)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "24 users already registered";
                    return serviceResponse;
                }

                foundEvent.RegisteredUserIds.Add(foundUser.Id);


                await _unitOfWork.Save();

                var registeredUserVm = UserMapper.RegisteredUserVm(foundUser);

                serviceResponse.Data = registeredUserVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RemoveUserFromEvent(RemoveUserFromEventDto removeUserFromEventDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == removeUserFromEventDto.EventId, includeProperties: "Teams");

                var foundUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == removeUserFromEventDto.UserId);
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event found.";
                    return serviceResponse;
                }

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by id";
                    return serviceResponse;
                }

                if (foundEvent.RegisteredUserIds.Exists(userId => userId == removeUserFromEventDto.UserId))
                {
                    var userToRemove = foundEvent.RegisteredUserIds.Find(userId => userId == foundUser.Id);
                    foundEvent.RegisteredUserIds.Remove(userToRemove);
                    await _unitOfWork.Save();
                    serviceResponse.Data = $"{foundUser.UserProfile.FirstName} {foundUser.UserProfile.LastName} has been removed from the event";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found to remove";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<EventVm>>> PublishedEventsByYear()
        {
            var serviceResponse = new ServiceResponse<List<EventVm>>();

            try
            {
                var publishedEvents = await _unitOfWork.Event.GetAll(e => e.IsPublished, orderBy: e => e.OrderByDescending(x => int.Parse(x.Year)));

                var eventVmList = await _eventMapper.EventVmList(publishedEvents.ToList());

                serviceResponse.Data = eventVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<bool>> DeleteEvent(int eventId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == eventId);
                if (foundEvent != null)
                {
                    await _unitOfWork.Event.RemoveAsync(foundEvent.EventId);
                    serviceResponse.Message = $"Event {foundEvent.Name} has been removed.";
                    serviceResponse.Data = true;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event found to remove";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AdminEventManagerVm>> EventForEventManager(int eventId)
        {
            var serviceResponse = new ServiceResponse<AdminEventManagerVm>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == eventId, "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found By Id";
                    return serviceResponse;
                }

                var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");
                var allUsersAsRegisteredUserVmList = allUsers.Select(user => new RegisteredUserVm
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Image = user.UserProfile.Image,
                    Username = user.UserName
                }).ToList();
                // HashSet<string> registeredUserIds = new HashSet<string>(foundEvent.RegisteredUserIds.Select(ru => ru.UserId));
                var registeredUserVmList = allUsersAsRegisteredUserVmList.Where(u => foundEvent.RegisteredUserIds.Contains(u.Id)).ToList();
                var unRegisteredUserVmList = allUsersAsRegisteredUserVmList.Where(u => !foundEvent.RegisteredUserIds.Contains(u.Id)).ToList();

                var teamListVm = await TeamMapper.TeamVmList(foundEvent.Teams);

                var adminEventManagerVm = new AdminEventManagerVm
                {
                    EventId = foundEvent.EventId,
                    Name = foundEvent.Name,
                    Location = _eventMapper.LocationVm(foundEvent.Location),
                    Itineraries = _eventMapper.ItineraryListVm(foundEvent.Itineraries),
                    RegisteredUsers = registeredUserVmList,
                    UnRegisteredUsers = unRegisteredUserVmList,
                    Teams = teamListVm,
                    Year = foundEvent.Year,
                    CreatedOn = foundEvent.CreatedOn,
                    IsCurrentYear = foundEvent.IsCurrentYear,
                    IsPublished = foundEvent.IsPublished,
                    UpdatedOn = foundEvent.UpdatedOn
                };


                serviceResponse.Data = adminEventManagerVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
