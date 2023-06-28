using BetSystem.BetSystemDbContext;
using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace BetSystem.Tests.BusinessLogic
{
    public class SportEventServiceTests
    {
       
        


        [Fact]
        public void TestPostSportEvent()
        {
            Mock<DbSet<SportEvent>>? mockSet = new Mock<DbSet<SportEvent>>();
            var betDbContextMock = new Mock<BetDbContext>();

            betDbContextMock
                .Setup(x => x.Events)
                .Returns(mockSet.Object);

            var service = new SportEventService(betDbContextMock.Object);
            var eventName = "TestEvent";

            service.PostSportEvent(new SportEventDto { Name = eventName });

            mockSet.Verify(m => m.Add(It.Is<SportEvent>(x => x.Name == eventName)), Times.Once());
            betDbContextMock.Verify(m => m.SaveChanges(), Times.Once());


        }

        [Fact]
        public void AddBetWhereTeamIdNotMatch()
        {
            //Arrange
            var mockDbContext = new Mock<BetDbContext>();
            

            var teamId = 1;
            var eventId = 1;
            var betOnEvent = new BetOnEventDto()
            {
                TeamId = teamId,    
                EventId = eventId
            };

            
            var team = new Team() { Id = 1000 };
            var sportEvent = new SportEvent() { Id = eventId};

            

            mockDbContext.Setup(x => x.Teams)
                .ReturnsDbSet(new List<Team>
                {
                    new Team() { Id = 1, Name = "aaa" }
                });

            mockDbContext.Setup(s => s.Events)
                .ReturnsDbSet(new List<SportEvent> 
                {
                    new SportEvent { Id = 1, Name = "bbb" }
                });

            mockDbContext.Setup(b => b.Bets)
                .ReturnsDbSet(new List<BetOnEvent>());

            var sportEventService = new SportEventService(mockDbContext.Object);


            //Act

            sportEventService.AddBetToSportEvent(betOnEvent);

            //Assert
            mockDbContext.Verify(x => x.Bets.Add(It.IsAny<BetOnEvent>()), Times.Once());
            mockDbContext.Verify(db => db.SaveChanges(), Times.Once());
            Assert.NotEqual(betOnEvent.TeamId,team.Id);
        }
    }
}
