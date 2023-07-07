using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Endpoint;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace BetSystem.Tests.Endpoint
{
    public class SportEventEndpointTests
    {
        private readonly Mock<ISportEventService> sportEventService;
        public SportEventEndpointTests()
        {
            sportEventService = new Mock<ISportEventService>();
        }

        [Fact]
        public void GetById_ShouldGetEventId()
        {
            var sportEventList = GetSportEventsData();

            sportEventService
                .Setup(x => x.GetById(It.Is<int>(x => x == 1)))
                .Returns(sportEventList[0]);

            IResult result = SportEventEndpoint.GetById(1, sportEventService.Object);

            Assert.IsType<Ok<SportEventDto>>(result);

            result = SportEventEndpoint.GetById(2, sportEventService.Object);
            Assert.IsType<NotFound<int>>(result);

        }

        [Fact]
        public void GetAllEvents_ShouldReturnSportEventList()
        {
            //Arrange
            var sportEventList = GetSportEventsData();
            sportEventService.Setup(x => x.GetAllEvents()).Returns(sportEventList);
            var sportEvent = sportEventService.Object;
            //Act
            var result = sportEvent.GetAllEvents();
            //Assert
            Assert.NotNull(result);
            Assert.Equal(GetSportEventsData().Count(), result.Count());
            Assert.True(sportEventList.Equals(result));
        }
        [Fact]
        public void PostSportEvent_ShouldAddSportEvent()
        {
            //Arrange
            var sportEventList = GetSportEventsData();
            sportEventService.Setup(x => x.PostSportEvent(sportEventList[2]));
            var sportEvent = sportEventService.Object;
            //Act
            sportEvent.PostSportEvent(sportEventList[2]);
            //Assert
            Assert.NotNull(sportEvent);

                
        }
        private List<SportEventDto> GetSportEventsData()
        {
            List<SportEventDto> sportEventData = new List<SportEventDto>
            { new SportEventDto { Id = 1 }, new SportEventDto {Id = 2 }, new SportEventDto {Id=3} };
            return sportEventData;
        }
    }
}
