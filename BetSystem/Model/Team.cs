using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace BetSystem.Model
{
    public class Team
    {
        [BsonId]
        public virtual int Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("results")]
        public List<EventResult> Results { get; set; } = new List<EventResult>();
    }
}
