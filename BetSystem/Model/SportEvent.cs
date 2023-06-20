namespace BetSystem.Model
{
    public class SportEvent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual List<Team> Teams { get; set; } = new List<Team>();
        public virtual List<EventResult> EventResults { get; set; } = new List<EventResult>();
        public virtual List<BetOnEvent> Bets { get; set; } = new List<BetOnEvent>();
    }
}
