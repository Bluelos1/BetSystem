namespace BetSystem.Model
{
    public class Team
    {

        public virtual int Id { get; set; }
        public string? Name { get; set; }
        public List<EventResult> Results { get; set; } = new List<EventResult>();
    }
}
