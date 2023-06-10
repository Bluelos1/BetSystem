using BetSystem.BetSystemDbContext;
using BetSystem.Contract;
using BetSystem.Model;

namespace BetSystem.BusinnessLogic
{
    public interface IEventResultService
    {
        void DeleteEventResultById(int id);
        List<EventResultDto> GetAllEvents();
        EventResultDto GetEventResultById(int id);
        void PostEventResult(EventResultDto eventResultDto);
    }

    public class EventResultService : IEventResultService
    {
        private readonly BetDbContext _dbContext;

        public EventResultService(BetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void PostEventResult(EventResultDto eventResultDto)
        {
            var eventResultToPost = new EventResult
            {
                BetOnResult = eventResultDto.BetOnResult
            };
            _dbContext.Results.Add(eventResultToPost);
            _dbContext.SaveChanges();
            eventResultDto.Id = eventResultToPost.Id;
        }

        public List<EventResultDto> GetAllEvents()
        {
            return _dbContext.Results.Select(x => new EventResultDto
            {
                Id = x.Id,
                BetOnResult = x.BetOnResult
            })
                .ToList();
        }

        public EventResultDto GetEventResultById(int id)
        {
            var betResult = _dbContext.Results.FirstOrDefault(x => x.Id == id);
            if (betResult == null) return null;
            return new EventResultDto
            {
                BetOnResult = betResult.BetOnResult,
            };
        }

        public void DeleteEventResultById(int id)
        {
            var result = _dbContext.Results.FirstOrDefault(x=>x.Id == id);
            _dbContext.Results.Remove(result);
            _dbContext.SaveChanges();
        }
    }
}
