namespace BetSystem.Model
{
    public class EventResult
    {
        public int Id { get; set; }
        public SportEvent Event { get; set; }
        public Team Team { get; set; }
        public BetResult BetOnResult { get; set; }
        public List<BetOnEvent> Bets { get; set; }

    }
}
