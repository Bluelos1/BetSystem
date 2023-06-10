using BetSystem.BetSystemDbContext;
using BetSystem.Contract;

using BetSystem.Model;
using FluentValidation;

namespace BetSystem.Contract.BusinnessLogic
{
    public interface IBetOnEventService
    {
        List<BetOnEventDto> GetAllBets();
        BetOnEventDto GetBetOnEventById(int id);
        void PlaceBetOnEvent(BetOnEventDto betOnEventDto);
        void DeleteBetOnEvent(int id);
    }

    public class BetOnEventService : IBetOnEventService
    {
        // Betting service porzadnie zrobione. CheckBet zla nazwa PlaceBet dlaczego zwraca BetOnEvent.
        // Fluent validation poczytac sobie. Dorobic testy . dlaczego w Team jest result. SportEvent nie wiadomo o co chodzi
        // validacja ma byc. Response Requesty moze zrobic aby bylo git I poymieniac nazwy niektorych rzeczy bo sa nieczytelned

        private readonly BetDbContext _dbContext;

        public BetOnEventService(BetDbContext betDbContext)
        {
            _dbContext = betDbContext;
        }


        public void PlaceBetOnEvent(BetOnEventDto betOnEventDto)
        {
            var betEventToSave = new BetOnEvent
            {
                BetOnResult = betOnEventDto.BetOnResult,
                Amount = betOnEventDto.Amount,
                Interest = betOnEventDto.Interest,
                AmountToPay = betOnEventDto.AmountToPay,
            };
            _dbContext.Bets.Add(betEventToSave);
            _dbContext.SaveChanges();

            betOnEventDto.Id = betEventToSave.Id;
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



    }
}

