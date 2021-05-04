using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class EventManagerService : IEventManagerService
    {
        private readonly AppDbContext _dbContext;

        public EventManagerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<int>> CreateEvent(CreateEventDto createEventDto)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var foundEventByYear = await _dbContext.Events.FirstOrDefaultAsync(e => e.Year == createEventDto.Year);
                if (foundEventByYear == null)
                {
                    var sandbaggerEvent = new Event
                    {
                        Name = createEventDto.Name,
                        Year = createEventDto.Year,
                        Location = new Location(),
                        Itineraries = new List<Itinerary>(),
                        CreatedOn = DateTime.Now
                    };


                    var createdEvent = await _dbContext.Events.AddAsync(sandbaggerEvent);
                    var eventResults = new EventResults
                    {
                        Event = createdEvent.Entity,
                        EventId = createdEvent.Entity.EventId,
                        Teams = new List<string>(),
                        ScrambleChamps = new List<string>(),
                        IsActive = false,
                    };
                    var eventGallery = new Gallery
                    {
                        EventId = createdEvent.Entity.EventId,
                        Event = createdEvent.Entity,
                        Name = createdEvent.Entity.Name,
                        Year = createdEvent.Entity.Year,
                        MainImg = "",
                        Images = new List<GalleryImage>()
                    };
                    await _dbContext.Galleries.AddAsync(eventGallery);
                    var powerRanking = new PowerRanking
                    {
                        Disclaimer = "",
                        CreatedOn = DateTime.Now,
                        Rankings = new List<Ranking>(),
                        Year = createdEvent.Entity.Year,
                        EventId = createdEvent.Entity.EventId,
                        Event = createdEvent.Entity
                    };
                    await _dbContext.PowerRankings.AddAsync(powerRanking);
                    var draft = new Draft
                    {
                        Event = createdEvent.Entity,
                        EventId = createdEvent.Entity.EventId,
                        DraftUsers = new List<DraftUser>(),
                        DraftCaptains = new List<DraftCaptain>()
                    };
                    await _dbContext.Drafts.AddAsync(draft);

                    createdEvent.Entity.EventResults = eventResults;

                    await _dbContext.SaveChangesAsync();


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

        public async Task<ServiceResponse<AdminEventManagerVm>> EventForEventManager(int eventId)
        {
            var serviceResponse = new ServiceResponse<AdminEventManagerVm>();

            try
            {
                var foundEvent = await _dbContext.Events.Include(e => e.Teams).FirstOrDefaultAsync(e => e.EventId == eventId);
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found By Id";
                    return serviceResponse;
                }

                var allUsers = await _dbContext.Users.Include(u => u.UserProfile).ToListAsync();
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
                        var foundCaptain = allUsers.FirstOrDefault(u => u.Id == team.CaptainId);

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
                            var foundTeamMember = allUsers.FirstOrDefault(u => u.Id == memberId);
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
                        State = foundEvent.Location.State,
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

        public async Task<ServiceResponse<RegisteredUserVm>> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto)
        {
            var serviceResponse = new ServiceResponse<RegisteredUserVm>();
            try
            {
                var foundUser = await _dbContext.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == registerUserForEventDto.UserId);

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by ID";
                    return serviceResponse;
                }

                var foundEvent = await _dbContext.Events.Include(u => u.Teams).FirstOrDefaultAsync(u => u.EventId == registerUserForEventDto.EventId);

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


                await _dbContext.SaveChangesAsync();

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
                var foundEvent = await _dbContext.Events.Include(u => u.Teams).FirstOrDefaultAsync(u => u.EventId == removeUserFromEventDto.EventId);

                var foundUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == removeUserFromEventDto.UserId);
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
                    await _dbContext.SaveChangesAsync();
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
    }
}
