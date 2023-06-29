using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BetSystem.Model;
using BetSystem.Contract;

namespace BetSystem.BusinnessLogic
{

    public interface IMongoDBService
    {
        void CreateTeam(TeamDto teamDto);
        public IEnumerable<TeamDto> GetTeams();
    }
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<TeamDto> _playlistCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(
                mongoDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDBSettings.Value.DatabaseName);

            _playlistCollection = mongoDatabase.GetCollection<TeamDto>(
                mongoDBSettings.Value.CollectionName);
        }


        public IEnumerable<TeamDto> GetTeams()
        {
            return _playlistCollection.Find(_ => true).ToList();
        }

        public void CreateTeam(TeamDto teamDto)
        {
            var teamToPost = new TeamDto
            {
                Id = teamDto.Id,
                Name = teamDto.Name,
            };
            _playlistCollection.InsertOne(teamToPost);
        }

    }
}
