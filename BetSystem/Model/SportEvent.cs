namespace BetSystem.Model
{
    public class SportEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<EventResult> EventResults { get; set; }
        public virtual List<BetOnEvent> Bets { get; set; }

    }
}
