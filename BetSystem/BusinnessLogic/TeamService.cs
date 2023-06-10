using BetSystem.BetSystemDbContext;
using BetSystem.Model;
using System.Runtime.CompilerServices;

namespace BetSystem.Contract.BusinnessLogic
{
    public interface ITeamService
    {
        List<TeamDto> GetAllTeams();
        TeamDto GetTeamById(int id);
        void PostTeam(TeamDto teamDto);
        void DeleteTeamById(int id);
        void PutTeam(int id, TeamDto teamDto);
    }

    public class TeamService : ITeamService
    {
        public readonly BetDbContext _dbContext;

        public TeamService(BetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void PostTeam(TeamDto teamDto)
        {
            var teamToPost = new Team
            {
                Name = teamDto.Name,
            };
            _dbContext.Teams.Add(teamToPost);
            _dbContext.SaveChanges();
            teamDto.Id = teamToPost.Id;
        }

        public List<TeamDto> GetAllTeams()
        {
            return _dbContext.Teams.Select(x => new TeamDto
            {   
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public TeamDto GetTeamById(int id)
        {
            var team = _dbContext.Teams.FirstOrDefault(x => x.Id == id);
            if (team == null) return null;
            return new TeamDto
            { Id = team.Id,
              Name = team.Name };

        }

        public void  DeleteTeamById(int id)
        {
            var team = _dbContext.Teams.FirstOrDefault(x=>x.Id == id);
            _dbContext.Teams.Remove(team);
            _dbContext.SaveChanges(); 
        }

        public void PutTeam(int id, TeamDto teamDto)
        {
            var team = _dbContext.Teams.FirstOrDefault(x=> x.Id == id);
            team.Name = teamDto.Name;
            _dbContext.SaveChanges();
            
        }
    }
}
