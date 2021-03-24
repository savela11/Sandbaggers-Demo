using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;


namespace Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;


        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<EventVm>> EventVm(Event evnt)
        {
            var serviceResponse = new ServiceResponse<EventVm>();

            try
            {
                var foundUsers = await _unitOfWork.User.GetAll(u => evnt.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");
                var registeredUserVmList = foundUsers.Select(u => new RegisteredUserVm
                {
                    Id = u.Id,
                    Username = u.UserName,
                    FullName = u.FullName,
                    Image = u.UserProfile.Image
                }).ToList();

                var teamVmList = new List<TeamVm>();

                foreach (var team in evnt.Teams)
                {
                    var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
                    if (string.IsNullOrEmpty(team.Name))
                    {
                        team.Name = team.TeamId.ToString();
                    }

                    if (!string.IsNullOrEmpty(team.CaptainId))
                    {
                        var foundCaptain = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
                        if (foundCaptain != null)
                        {
                            teamVm.Captain.Id = foundCaptain.Id;
                            teamVm.Captain.Image = foundCaptain.UserProfile.Image;
                            teamVm.Captain.FullName = foundCaptain.FullName;
                        }
                    }

                    teamVm.TeamMembers = new List<TeamMemberVm>();
                    if (team.TeamMemberIds.Count > 0)
                    {
                        foreach (var memberId in team.TeamMemberIds)
                        {
                            var foundTeamMember = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == memberId, includeProperties: "UserProfile");
                            if (foundTeamMember != null)
                            {
                                teamVm.TeamMembers.Add(new TeamMemberVm
                                {
                                    Id = foundTeamMember.Id,
                                    Image = foundTeamMember.UserProfile.Image,
                                    FullName = foundTeamMember.FullName
                                });
                            }
                        }
                    }

                    teamVmList.Add(teamVm);
                }


                var eventVm = new EventVm
                {
                    EventId = evnt.EventId,
                    Name = evnt.Name,
                    Location = new LocationVm
                    {
                        Name = evnt.Location.Name,
                        StreetNumbers = evnt.Location.StreetNumbers,
                        StreetName = evnt.Location.StreetName,
                        City = evnt.Location.City,
                        PostalCode = evnt.Location.PostalCode
                    },
                    Itineraries = evnt.Itineraries.Select(i => new ItineraryVm
                    {
                        Day = i.Description,
                        Time = i.Time,
                        Description = i.Description
                    }).ToList(),
                    RegisteredUsers = registeredUserVmList,
                    Teams = teamVmList,
                    Year = evnt.Year,
                    CreatedOn = evnt.CreatedOn,
                    IsCurrentYear = evnt.IsCurrentYear,
                    IsPublished = evnt.IsPublished,
                    UpdatedOn = evnt.UpdatedOn
                };
                serviceResponse.Data = eventVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<EventVm>>> EventVmList(List<Event> evnts)
        {
            var serviceResponse = new ServiceResponse<List<EventVm>>();

            try
            {
                var eventVmList = new List<EventVm>();
                foreach (var evnt in evnts)

                {
                    var eventVmResponse = await EventVm(evnt);
                    if (eventVmResponse.Success)
                    {
                        eventVmList.Add(eventVmResponse.Data);
                    }
                }

                serviceResponse.Data = eventVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> CreateEvent(CreateEventDto createEventDto)
        {
            var serviceResponse = new ServiceResponse<int>();
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


                    serviceResponse.Data = 1;
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

        public async Task<ServiceResponse<List<Event>>> Events()
        {
            var serviceResponse = new ServiceResponse<List<Event>>();
            try
            {
                var foundEvents = await _unitOfWork.Event.GetAll(orderBy: e => e.OrderBy(d => d.Year), includeProperties: "Teams");


                serviceResponse.Data = foundEvents.ToList();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<Event>> GetEventById(int id)
        {
            var serviceResponse = new ServiceResponse<Event>();
            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == id, includeProperties: "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event found by Id";
                    return serviceResponse;
                }


                serviceResponse.Data = foundEvent;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<Event>> UpdateEvent(EventVm sandbaggerEventVm)
        {
            var serviceResponse = new ServiceResponse<Event>();

            try
            {
                var foundActiveAndPublishedEvents = await _unitOfWork.Event.GetAll(includeProperties: "Teams");
                var foundPublishedEventsList = foundActiveAndPublishedEvents.ToList();
                var foundEvent = foundPublishedEventsList.FirstOrDefault(e => e.EventId == sandbaggerEventVm.EventId);
                // var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == sandbaggerEventVm.EventId, includeProperties: "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found";
                    return serviceResponse;
                }

                foundEvent.Name = sandbaggerEventVm.Name;
                foundEvent.Itineraries = sandbaggerEventVm.Itineraries.Select(i => new Itinerary
                {
                    Day = i.Day,
                    Time = i.Time,
                    Description = i.Description
                }).ToList();
                foundEvent.Location = new Location
                {
                    Name = sandbaggerEventVm.Location.Name,
                    StreetNumbers = sandbaggerEventVm.Location.StreetNumbers,
                    StreetName = sandbaggerEventVm.Location.StreetName,
                    City = sandbaggerEventVm.Location.City,
                    PostalCode = sandbaggerEventVm.Location.PostalCode
                };
                foundEvent.UpdatedOn = DateTime.UtcNow;


                //check if there is already an event active or current year
                if (sandbaggerEventVm.IsPublished || sandbaggerEventVm.IsCurrentYear)
                {
                    var activeNotCurrentEvent = foundPublishedEventsList.FirstOrDefault(e => e.IsPublished && e.EventId != foundEvent.EventId);

                    // var activeOrPublished = await _unitOfWork.Event.CheckPublishedOrActive(sandbaggerEventVm.EventId);
                    if (activeNotCurrentEvent != null)
                    {
                        activeNotCurrentEvent.IsCurrentYear = false;
                        activeNotCurrentEvent.IsPublished = false;
                        // serviceResponse.Success = false;
                        // serviceResponse.Message = "There is already an event that is active or set as current year.";
                        // return serviceResponse;
                    }

                }

                foundEvent.IsPublished = sandbaggerEventVm.IsPublished;
                foundEvent.IsCurrentYear = sandbaggerEventVm.IsCurrentYear;

                await _unitOfWork.Save();
                serviceResponse.Data = foundEvent;
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

                var registeredUserVm = new RegisteredUserVm
                {
                    Id = foundUser.Id,
                    Username = foundUser.UserName,
                    FullName = foundUser.FullName,
                    Image = foundUser.UserProfile.Image
                };


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
                    serviceResponse.Data = $"{foundUser.FullName} has been removed from the event";
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

        public async Task<ServiceResponse<List<Event>>> PublishedEventsByYear()
        {
            var serviceResponse = new ServiceResponse<List<Event>>();

            try
            {
                var publishedEvents = await _unitOfWork.Event.GetAll(e => e.IsPublished, orderBy: e => e.OrderByDescending(x => x.Year));


                serviceResponse.Data = publishedEvents.ToList();
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

                var teamVmList = new List<TeamVm>();

                foreach (var team in foundEvent.Teams)
                {
                    var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
                    if (string.IsNullOrEmpty(team.Name))
                    {
                        team.Name = team.TeamId.ToString();
                    }

                    if (!string.IsNullOrEmpty(team.CaptainId))
                    {
                        var foundCaptain = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
                        if (foundCaptain != null)
                        {
                            teamVm.Captain.Id = foundCaptain.Id;
                            teamVm.Captain.Image = foundCaptain.UserProfile.Image;
                            teamVm.Captain.FullName = foundCaptain.FullName;
                        }
                    }

                    teamVm.TeamMembers = new List<TeamMemberVm>();
                    if (team.TeamMemberIds.Count > 0)
                    {
                        foreach (var memberId in team.TeamMemberIds)
                        {
                            var foundTeamMember = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == memberId, includeProperties: "UserProfile");
                            if (foundTeamMember != null)
                            {
                                teamVm.TeamMembers.Add(new TeamMemberVm
                                {
                                    Id = foundTeamMember.Id,
                                    Image = foundTeamMember.UserProfile.Image,
                                    FullName = foundTeamMember.FullName
                                });
                            }
                        }
                    }

                    teamVmList.Add(teamVm);
                }


                var adminEventManagerVm = new AdminEventManagerVm
                {
                    EventId = foundEvent.EventId,
                    Name = foundEvent.Name,
                    Location = new LocationVm
                    {
                        Name = foundEvent.Location.Name,
                        StreetNumbers = foundEvent.Location.StreetNumbers,
                        StreetName = foundEvent.Location.StreetName,
                        City = foundEvent.Location.City,
                        PostalCode = foundEvent.Location.PostalCode
                    },
                    Itineraries = foundEvent.Itineraries.Select(i => new ItineraryVm
                    {
                        Day = i.Description,
                        Time = i.Time,
                        Description = i.Description
                    }).ToList(),
                    RegisteredUsers = registeredUserVmList,
                    UnRegisteredUsers = unRegisteredUserVmList,
                    Teams = teamVmList,
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
