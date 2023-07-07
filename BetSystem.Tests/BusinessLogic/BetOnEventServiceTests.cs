using BetSystem.BetSystemDbContext;
using Moq;
using Moq.EntityFrameworkCore;

namespace BetSystem.Tests.BusinessLogic
{
    public class BetOnEventServiceTests
    {
        private readonly Mock<BetDbContext> dbContextMock;
        private readonly BetOnEventService betOnEventService;

        public BetOnEventServiceTests()
        {
            dbContextMock = new Mock<BetDbContext>();
            betOnEventService = new BetOnEventService(dbContextMock.Object);
        }

        [Fact]
        public void GetAllBets_ShouldReturnAllBets()
        {
            // Arrange
            

            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, BetOnResult = BetResult.WIN, Amount = 10, Interest = 3, AmountToPay = 15 },
            new BetOnEvent { Id = 2, BetOnResult = BetResult.LOSE, Amount = 20, Interest = 2, AmountToPay = 5 }
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            // Act
            var result = betOnEventService.GetAllBets();

            // Assert
            Assert.Equal(bets.Count, result.Count);
            Assert.Equal(bets[0].Id, result[0].Id);
            Assert.Equal(bets[0].BetOnResult, result[0].BetOnResult);
            Assert.Equal(bets[0].Amount, result[0].Amount);
            Assert.Equal(bets[0].Interest, result[0].Interest);
            Assert.Equal(bets[0].AmountToPay, result[0].AmountToPay);
            Assert.Equal(bets[1].Id, result[1].Id);
            Assert.Equal(bets[1].BetOnResult, result[1].BetOnResult);
            Assert.Equal(bets[1].Amount, result[1].Amount);
            Assert.Equal(bets[1].Interest, result[1].Interest);
            Assert.Equal(bets[1].AmountToPay, result[1].AmountToPay);
        }

        [Fact]
        public void GetBetOnEventById_ExistingId_ShouldReturnBet()
        {
            // Arrange
            var bets = new List<BetOnEvent>
        {
                new BetOnEvent {Id = 1},
                new BetOnEvent { Id = 2}
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            // Act
            var result = betOnEventService.GetBetOnEventById(1);

            // Assert
            Assert.Equal(bets[0].Id, result.Id);
            Assert.NotEqual(bets[1].Id, result.Id);
        }

        [Fact]
        public void GetBetOnEventById_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, BetOnResult = BetResult.WIN, Amount = 10, Interest = 2, AmountToPay = 15 },
            new BetOnEvent { Id = 2, BetOnResult = BetResult.LOSE, Amount = 20, Interest = 3, AmountToPay = 5 }
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            // Act
            var result = betOnEventService.GetBetOnEventById(3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteBetOnEvent_ExistingId_ShouldRemoveBetFromDbContext()
        {
            // Arrange
            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, BetOnResult = BetResult.WIN, Amount = 10, Interest = 4, AmountToPay = 15 },
            new BetOnEvent { Id = 2, BetOnResult = BetResult.LOSE, Amount = 20, Interest = 2, AmountToPay = 5 }
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            // Act
            betOnEventService.DeleteBetOnEvent(3);

            // Assert
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
            Assert.Equal(2, bets.Count); 
            Assert.Equal(1, bets[0].Id); 
        }

        [Fact]
        public void PutBetOnEvent_ExistingId_ShouldUpdateBetInDbContext()
        {
            // Arrange
            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, BetOnResult = BetResult.WIN, Amount = 10, Interest = 4, AmountToPay = 15 },
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            var updatedBet = new BetOnEventDto
            {
                Id = 1,
                BetOnResult = BetResult.WIN,
                Amount = 10,
                Interest = 4,
                AmountToPay = 15
            };

            // Act
            betOnEventService.PutBetOnEvent(bets[0].Id, updatedBet);

            // Assert
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
            Assert.Equal(updatedBet.Id, bets[0].Id);
            Assert.Equal(updatedBet.BetOnResult, bets[0].BetOnResult); 
            Assert.Equal(updatedBet.Amount, bets[0].Amount);
            Assert.Equal(updatedBet.Interest, bets[0].Interest);
            Assert.Equal(updatedBet.AmountToPay, bets[0].AmountToPay);
        }

        [Fact]
        public void GetStatistics_ShouldReturnStatsDtoWithCorrectCounts()
        {
            // Arrange
            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, BetOnResult = BetResult.WIN, Amount = 10, Interest = 4, AmountToPay = 15 },
            new BetOnEvent { Id = 2, BetOnResult = BetResult.LOSE, Amount = 20, Interest = 2, AmountToPay = 5 },
            new BetOnEvent { Id = 3, BetOnResult = BetResult.WIN, Amount = 30, Interest = 3, AmountToPay = 50 },
            new BetOnEvent { Id = 4, BetOnResult = BetResult.DRAW, Amount = 15, Interest = 5, AmountToPay = 15 }
        };
            dbContextMock.Setup(db => db.Bets).ReturnsDbSet(bets);

            // Act
            var result = betOnEventService.GetStatistics();

            // Assert
            Assert.Equal(2, result.WinCount);
            Assert.Equal(1, result.LoseCount);
            Assert.Equal(1, result.DrawCount);
        }
    }
}
    

