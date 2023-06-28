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
        [Fact]
        public void TestGetById()
        {
            var serviceMock = new Mock<ISportEventService>();
            serviceMock
                .Setup(x => x.GetById(It.Is<int>(x => x == 1)))
                .Returns(new SportEventDto { Id = 1 });

            IResult result = SportEventEndpoint.GetById(1, serviceMock.Object);

            Assert.IsType<Ok<SportEventDto>>(result);

            result = SportEventEndpoint.GetById(2, serviceMock.Object);
            Assert.IsType<NotFound<int>>(result);

        }

    }
}
