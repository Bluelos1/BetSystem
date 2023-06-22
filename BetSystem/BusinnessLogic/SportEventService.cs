using BetSystem.BetSystemDbContext;
using BetSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BetSystem.Contract.BusinnessLogic
{
    public interface ISportEventService
    {
        void AddTeamToSportEvent(int id, IdRequestDto idRequestDto);
        List<TeamDto> GetTeamsFromSportEvent(int id);
        void DeleteById(int id);
        List<SportEventDto> GetAllEvents();
        SportEventDto GetById(int id);
        void PostSportEvent(SportEventDto sportEventDto);
        void AddBetToSportEvent(BetOnEventDto betOnEventDto);
        List<BetOnEventDto> GetBetsFromSportEvent(int id);
        void AddResultToSportEvent(int id, EventResultDto eventResultDto);
        List<EventResultDto> GetResultsFromSportEvent(int id);
        void PutSportEvent(int id, SportEventDto sportEventDto);
    }


    public class SportEventService : ISportEventService
    {
        public readonly BetDbContext _dbContext;

        public SportEventService(BetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void PostSportEvent(SportEventDto sportEventDto)
        {
            var sportEventToPost = new SportEvent
            {
                Name = sportEventDto.Name
            };
            _dbContext.Events.Add(sportEventToPost);
            _dbContext.SaveChanges();

            sportEventDto.Id = sportEventToPost.Id;
        }

        public List<SportEventDto> GetAllEvents()
        {
            return _dbContext.Events.Select(x => new SportEventDto
            {
                Id = x.Id,
                Name = x.Name
            })
                .ToList();
        }

        public SportEventDto GetById(int id)
        {
            var sportEvent = _dbContext.Events.FirstOrDefault(x => x.Id == id);
            if (sportEvent == null) { return null; }
            return new SportEventDto
            {
                Id = sportEvent.Id,
                Name = sportEvent.Name
            };
        }
        public void DeleteById(int id)
        {
            var sportEvent = _dbContext.Events.FirstOrDefault(x => x.Id == id);
            _dbContext.Events.Remove(sportEvent);
            _dbContext.SaveChanges();
        }

        public void AddTeamToSportEvent(int id, IdRequestDto idRequestDto)
        {
            var team = _dbContext.Teams.FirstOrDefault(x => x.Id == idRequestDto.Id);
            _dbContext
                .Events.FirstOrDefault(x => x.Id == id)
                .Teams.Add(team);
            _dbContext.SaveChanges();
        }

        public List<TeamDto> GetTeamsFromSportEvent(int id)
        {
            return _dbContext.Events
                .Include(x => x.Teams)
                .FirstOrDefault(x => x.Id == id)
                .Teams
                .Select(x => new TeamDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
        }

        public void AddBetToSportEvent(BetOnEventDto betOnEventDto)
        {
            var betEventToSave = new BetOnEvent
            {
                BetOnResult = betOnEventDto.BetOnResult,
                Amount = betOnEventDto.Amount,
                Interest = betOnEventDto.Interest,
                AmountToPay = betOnEventDto.AmountToPay,
                Team = _dbContext.Teams.First(x => x.Id == betOnEventDto.TeamId),
                Event = _dbContext.Events.First(x => x.Id == betOnEventDto.EventId)
            };
            _dbContext.Bets.Add(betEventToSave);
            _dbContext.SaveChanges();
        }

        public List<BetOnEventDto> GetBetsFromSportEvent(int id)
        {
            return _dbContext.Events
                .Include(x => x.Bets)
                .FirstOrDefault(x => x.Id == id)
                .Bets
                .Select(x => new BetOnEventDto
                {
                    Id = x.Id,
                    BetOnResult = x.BetOnResult,
                    Amount = x.Amount,
                    Interest = x.Interest,
                    AmountToPay = x.AmountToPay,
                }).ToList();
        }

        public void AddResultToSportEvent(int id, EventResultDto eventResultDto)
        {
            var eventResult = new EventResult
            {
                BetOnResult = eventResultDto.BetOnResult,
                Team = _dbContext.Teams.First(x => x.Id == eventResultDto.TeamId),
                Event = _dbContext.Events.First(x => x.Id == id)
            };
            _dbContext.Bets
                .Include(x => x.Team)
                .Where(x => x.Event.Id == id)
                .ToList()
                .ForEach(x => x.UpdateAmountToPay(eventResult));
            _dbContext.Results.Add(eventResult);
            _dbContext.SaveChanges();
        }

        public List<EventResultDto> GetResultsFromSportEvent(int id)
        {
            return _dbContext.Events
                .Include(x => x.EventResults)
                .FirstOrDefault(x => x.Id == id)
                .EventResults
                .Select(x => new EventResultDto
                {
                    Id = x.Id,
                    BetOnResult = x.BetOnResult,
                })
                .ToList();
            
        }

        public void PutSportEvent(int id, SportEventDto sportEventDto)
        {
            var sportEvent = _dbContext.Events.FirstOrDefault(x => x.Id == id);
            sportEvent.Name = sportEventDto.Name;
            _dbContext.SaveChanges();
        }

    }
}
