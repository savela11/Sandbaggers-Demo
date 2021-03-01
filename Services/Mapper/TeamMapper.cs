using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class TeamMapper
    {
        public static IUnitOfWork OfWork { get; }

        public static async Task<List<TeamVm>> TeamVmList(IEnumerable<Team> teamList)
        {
            var teamVmList = new List<TeamVm>();
            foreach (var team in teamList)
            {
                var teamVm = await TeamVm(team);
                teamVmList.Add(teamVm);
            }

            return teamVmList;
        }

        public static async Task<List<TeamVm>> TeamVmList(IEnumerable<string> teamIds)
        {
            var teams = new List<Team>();
            foreach (var id in teamIds)
            {
                var team = await OfWork.Team.GetFirstOrDefault(t => t.TeamId == int.Parse(id));

                teams.Add(team);
            }

            var teamVmList = await TeamVmList(teams);

            return teamVmList;
        }

        public static async Task<TeamVm> TeamVm(Team team)
        {
            var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
            if (string.IsNullOrEmpty(team.Name))
            {
                team.Name = team.TeamId.ToString();
            }

            if (!string.IsNullOrEmpty(team.CaptainId))
            {
                var foundCaptain = await OfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
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
                    var foundTeamMember = await OfWork.User.GetFirstOrDefault(u => u.Id == memberId, includeProperties: "UserProfile");
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

            return teamVm;
        }


    }
}
