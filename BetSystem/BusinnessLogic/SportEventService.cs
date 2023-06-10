using BetSystem.BetSystemDbContext;
using BetSystem.Contract;
using BetSystem.Model;

namespace BetSystem.Contract.BusinnessLogic
{
    public interface ISportEventService
    {
        void DeleteById(int id);
        List<SportEventDto> GetAllEvents();
        SportEventDto GetById(int id);
        void PostSportEvent(SportEventDto sportEventDto);
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
            if(sportEvent == null) { return null; } 
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
    }
}
