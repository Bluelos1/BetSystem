using BetSystem.BetSystemDbContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BetSystem.Tests.BusinessLogic
{
    public class SportEventServiceTests
    {
       private readonly Mock<BetDbContext> dbContext;
        private readonly SportEventService sportEventService;


        public SportEventServiceTests()
        {
            dbContext = new Mock<BetDbContext>();
            sportEventService = new SportEventService(dbContext.Object);
        }



        [Fact]
        public void PostSportEvent_ShouldCreateSportEvent()
        {
            //Arrange
            var sportEventDbSetMock = new Mock<DbSet<SportEvent>>();
            dbContext.Setup(dbContext => dbContext.Events).Returns(sportEventDbSetMock.Object);
            var sportEvent = new SportEventDto { Name = "event" };

            //Act
            sportEventService.PostSportEvent(sportEvent);

            //Assert
            sportEventDbSetMock.Verify(dbSet => dbSet.Add(It.IsAny<SportEvent>()), Times.Once);
            dbContext.Verify(db => db.SaveChanges(), Times.Once);
            Assert.NotNull(sportEvent.Id);

        }

        [Fact]
        public void AddBetToSportEvent_ShouldAddBetWhereTeamIdMatch()
        {
            //Arrange
            

            var teamId = 1;
            var eventId = 1;
            var betOnEvent = new BetOnEventDto()
            {
                TeamId = teamId,    
                EventId = eventId
            };

            
            var team = new Team() { Id = 1 };
            var sportEvent = new SportEvent() { Id = eventId};

            

            dbContext.Setup(x => x.Teams)
                .ReturnsDbSet(new List<Team>
                {
                    new Team() { Id = 1, Name = "aaa" }
                });

            dbContext.Setup(s => s.Events)
                .ReturnsDbSet(new List<SportEvent> 
                {
                    new SportEvent { Id = 1, Name = "bbb" }
                });

            dbContext.Setup(b => b.Bets)
                .ReturnsDbSet(new List<BetOnEvent>());



            //Act

            sportEventService.AddBetToSportEvent(betOnEvent);

            //Assert
            dbContext.Verify(x => x.Bets.Add(It.IsAny<BetOnEvent>()), Times.Once());
            dbContext.Verify(db => db.SaveChanges(), Times.Once());
            Assert.Equal(betOnEvent.TeamId,team.Id);
        }



       
        [Fact]
        public void GetAllEvents_Should_ReturnAllSportEvents()
        {
            // Arrange
            
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1" },
            new SportEvent { Id = 2, Name = "Event 2" },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetAllEvents();

            // Assert
            Assert.Equal(events.Count, result.Count);
            Assert.Equal(events[0].Id, result[0].Id);
            Assert.Equal(events[0].Name, result[0].Name);
            Assert.Equal(events[1].Id, result[1].Id);
            Assert.Equal(events[1].Name, result[1].Name);
        }

        [Fact]
        public void GetById_Should_ReturnSportEventWithMatchingId()
        {
            // Arrange
            
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1" },
            new SportEvent { Id = 2, Name = "Event 2" },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(events[1].Id, result.Id);
            Assert.Equal(events[1].Name, result.Name);
        }

        [Fact]
        public void GetById_Should_ReturnNull_WhenSportEventNotFound()
        {
            // Arrange
          
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1" },
            new SportEvent { Id = 2, Name = "Event 2" },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetById(3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteById_Should_RemoveSportEventFromDbContext()
        {
            // Arrange
            
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1" },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            sportEventService.DeleteById(1);

            // Assert
            dbContext.Verify(db => db.SaveChanges(), Times.Once);
            dbContext.Verify(db => db.Events.Remove(It.IsAny<SportEvent>()), Times.Once);
        }

        [Fact]
        public void AddTeamToSportEvent_Should_AddTeamToSportEvent()
        {
            // Arrange
            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
        };
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1", Teams = teams },
        };
            dbContext.Setup(db => db.Teams).ReturnsDbSet(teams);
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            sportEventService.AddTeamToSportEvent(events[0].Id, new IdRequestDto { Id = 1 });

            // Assert
            dbContext.Verify(db => db.SaveChanges(), Times.Once);
            Assert.Contains(teams[0], events[0].Teams);
        }

        [Fact]
        public void GetTeamsFromSportEvent_Should_ReturnTeamsForSportEvent()
        {
            // Arrange
            
            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
            new Team { Id = 2, Name = "Team 2" },
        };
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1", Teams = new List<Team> { teams[0], teams[1] } },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetTeamsFromSportEvent(events[0].Id);

            // Assert
            Assert.Equal(teams.Count, result.Count);
            Assert.Equal(teams[0].Id, result[0].Id);
            Assert.Equal(teams[0].Name, result[0].Name);
            Assert.Equal(teams[1].Id, result[1].Id);
            Assert.Equal(teams[1].Name, result[1].Name);
        }

       

        [Fact]
        public void GetBetsFromSportEvent_Should_ReturnBetsForSportEvent()
        {
            // Arrange
            
            var events = new List<SportEvent>
            {
                new SportEvent 
                {
                    Id = 1, Name = "Event", Bets = new List<BetOnEvent> 
                    {
                        new BetOnEvent 
                        {
                            Id = 1, Amount = 100 
                        } 
                    } 
                },
            };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetBetsFromSportEvent(1);

            // Assert
            Assert.Equal(events[0].Bets.Count, result.Count);
            Assert.Equal(events[0].Bets[0].Id, result[0].Id);
            Assert.Equal(events[0].Bets[0].Amount, result[0].Amount);
        }

        [Fact]
        public void AddResultToSportEvent_Should_AddResultToSportEvent()
        {
            // Arrange
           
            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team" },
        };
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event" },
        };
            dbContext.Setup(db => db.Teams).ReturnsDbSet(teams);
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            var bets = new List<BetOnEvent>
        {
            new BetOnEvent { Id = 1, Amount = 100, Team = teams[0], Event = events[0] },
        };
            dbContext.Setup(db => db.Bets).ReturnsDbSet(bets);

            var eventResultDto = new EventResultDto
            {
                BetOnResult = BetResult.WIN,
                TeamId = 1,
            };


            // Act
            sportEventService.AddResultToSportEvent(events[0].Id, eventResultDto);

            // Assert
            dbContext.Verify(db => db.SaveChanges(), Times.Once);
            dbContext.Verify(db => db.Results.Add(It.IsAny<EventResult>()), Times.Once);
        }

        [Fact]
        public void GetResultsFromSportEvent_Should_ReturnResultsForSportEvent()
        {
            // Arrange
           
            var events = new List<SportEvent>
        {
            new SportEvent 
            { 
                Id = 1, Name = "Event 1",
                EventResults = new List<EventResult>
                {
                    new EventResult 
                    { 
                        Id = 1, BetOnResult = BetResult.WIN 
                    } 
                } 
            },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            // Act
            var result = sportEventService.GetResultsFromSportEvent(1);

            // Assert
            Assert.Equal(events[0].EventResults.Count, result.Count);
            Assert.Equal(events[0].EventResults[0].Id, result[0].Id);
            Assert.Equal(events[0].EventResults[0].BetOnResult, result[0].BetOnResult);
        }

        [Fact]
        public void PutSportEvent_Should_UpdateSportEventInDbContext()
        {
            // Arrange
            
            var events = new List<SportEvent>
        {
            new SportEvent { Id = 1, Name = "Event 1" },
        };
            dbContext.Setup(db => db.Events).ReturnsDbSet(events);

            var updatedEvent = new SportEventDto { Id = 1, Name = "Updated Event" };

            // Act
            sportEventService.PutSportEvent(events[0].Id, updatedEvent);

            // Assert
            dbContext.Verify(db => db.SaveChanges(), Times.Once);
            Assert.Equal(updatedEvent.Name, events[0].Name);
        }

        
    }
}

