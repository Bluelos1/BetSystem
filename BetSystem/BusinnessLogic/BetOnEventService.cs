using BetSystem.BetSystemDbContext;

using BetSystem.Model;

namespace BetSystem.Contract.BusinnessLogic
{
    public interface IBetOnEventService
    {
        List<BetOnEventDto> GetAllBets();
        BetOnEventDto GetBetOnEventById(int id);
        void DeleteBetOnEvent(int id);
        void PutBetOnEvent(int id, BetOnEventDto betOnEvent);
        StatsDto GetStatistics();
    }

    public class BetOnEventService : IBetOnEventService
    {

        private readonly BetDbContext _dbContext;

        public BetOnEventService(BetDbContext betDbContext)
        {
            _dbContext = betDbContext;
        }




        public List<BetOnEventDto> GetAllBets()
        {
            return _dbContext.Bets.Select(x => new BetOnEventDto
            {
                Id = x.Id,
                BetOnResult = x.BetOnResult,
                Amount = x.Amount,
                Interest = x.Interest,
                AmountToPay = x.AmountToPay
            })
                .ToList();
        }

        public BetOnEventDto GetBetOnEventById(int id)
        {
            var betEvent = _dbContext.Bets.FirstOrDefault(x => x.Id == id);
            if (betEvent == null) return null;
            return new BetOnEventDto
            {
                BetOnResult = betEvent.BetOnResult,
                Amount = betEvent.Amount,
                Interest = betEvent.Interest,
                AmountToPay = betEvent.AmountToPay
            };

        }

        public void DeleteBetOnEvent(int id)
        {
            var betEvent = _dbContext.Bets.FirstOrDefault(x => x.Id == id);
            _dbContext.Bets.Remove(betEvent);
            _dbContext.SaveChanges();
        }

        public void PutBetOnEvent(int id, BetOnEventDto betOnEvent)
        {
            var bet = _dbContext.Bets.FirstOrDefault(x => x.Id == id);
            bet.BetOnResult = betOnEvent.BetOnResult;
            bet.Amount = betOnEvent.Amount;
            bet.Interest = betOnEvent.Interest;
            bet.AmountToPay = betOnEvent.AmountToPay;
            _dbContext.SaveChanges();
        }

        public StatsDto GetStatistics()
        {
            var winCount = _dbContext.Bets.Where(x => x.BetOnResult == BetResult.WIN && x.Event.EventResults.Any()).Count();
            var loseCount = _dbContext.Bets.Where(x => x.BetOnResult == BetResult.LOSE && x.Event.EventResults.Any()).Count();
            var drawCount = _dbContext.Bets.Where(x => x.BetOnResult == BetResult.DRAW && x.Event.EventResults.Any()).Count();
            return new StatsDto
            {
                WinCount = winCount,
                LoseCount = loseCount,
                DrawCount = drawCount
            };
        }
    }
}

