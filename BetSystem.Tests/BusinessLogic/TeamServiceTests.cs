using BetSystem.BetSystemDbContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BetSystem.Tests.BusinessLogic
{
    public class TeamServiceTests
    {
        private readonly Mock<BetDbContext> dbContextMock;
        private readonly TeamService teamService;

        public TeamServiceTests()
        {
            dbContextMock = new Mock<BetDbContext>();
            teamService = new TeamService(dbContextMock.Object);
        }
        [Fact]
        public void PostTeam_ShouldAddNewTeam()
        {
            var teamsDbSetMock = new Mock<DbSet<Team>>();
            dbContextMock.Setup(db => db.Teams).Returns(teamsDbSetMock.Object);
            var teamDto = new TeamDto { Name = "Test Team" };

            // Act
            teamService.PostTeam(teamDto);

            // Assert
            teamsDbSetMock.Verify(dbSet => dbSet.Add(It.IsAny<Team>()), Times.Once);
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
            Assert.NotNull(teamDto.Id);
        }

        [Fact]
        public void GetAllTeams_ShouldReturnAllTeams()
        {
            // Arrange
            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
            new Team { Id = 2, Name = "Team 2" }
        };
            dbContextMock.Setup(db => db.Teams).ReturnsDbSet(teams);

            // Act
            var result = teamService.GetAllTeams();

            // Assert
            Assert.Equal(teams.Count, result.Count);
            Assert.Equal(teams[0].Id, result[0].Id);
            Assert.Equal(teams[0].Name, result[0].Name);
            Assert.Equal(teams[1].Id, result[1].Id);
            Assert.Equal(teams[1].Name, result[1].Name);
        }

        [Fact]
        public void GetTeamById_NonExistingId_ShouldReturnNull()
        {
            // Arrange

            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
            new Team { Id = 2, Name = "Team 2" },
            new Team { Id = 3, Name = "Team 3" }
        };
            dbContextMock.Setup(db => db.Teams).ReturnsDbSet(teams);

            // Act
            var result = teamService.GetTeamById(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteTeamById_ExistingId_ShouldRemoveTeamFromDbContext()
        {
            // Arrange
            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
            new Team { Id = 2, Name = "Team 2" },
            new Team { Id = 3, Name = "Team 3" }
        };
            dbContextMock.Setup(db => db.Teams).ReturnsDbSet(teams);

            // Act
            teamService.DeleteTeamById(2);

            // Assert
            dbContextMock.Verify(db => db.Teams.Remove(It.IsAny<Team>()), Times.Once);
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
        }

        [Fact]
        public void PutTeam_ShouldUpdateTeamInDbContext()
        {
            // Arrange

            var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Team 1" },
            new Team { Id = 2, Name = "Team 2" },
            new Team { Id = 3, Name = "Team 3" }
        };
            dbContextMock.Setup(db => db.Teams).ReturnsDbSet(teams);

            var updatedTeamDto = new TeamDto { Id = 2, Name = "Updated Team 2" };

            // Act
            teamService.PutTeam(2, updatedTeamDto);

            // Assert
            var updatedTeam = teams.FirstOrDefault(t => t.Id == 2);
            Assert.NotNull(updatedTeam);
            Assert.Equal("Updated Team 2", updatedTeam.Name);
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
        }
    }
}
