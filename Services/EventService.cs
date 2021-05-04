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
                        State = evnt.Location.State,
                        City = evnt.Location.City,
                        PostalCode = evnt.Location.PostalCode
                    },
                    Itineraries = evnt.Itineraries.Select(i => new ItineraryVm
                    {
                        Day = i.Day,
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
                    State = sandbaggerEventVm.Location.State,
                    City = sandbaggerEventVm.Location.City,
                    PostalCode = sandbaggerEventVm.Location.PostalCode
                };


                foundEvent.UpdatedOn = DateTime.Now;


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


    }
}
