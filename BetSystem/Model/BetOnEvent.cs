namespace BetSystem.Model
{
    public class BetOnEvent
    {

        public int Id { get; set; }
        public SportEvent Event { get; set; }
        public Team Team { get; set; }
        public BetResult BetOnResult { get; set; }
        public int Amount { get; set; }
        public int Interest { get; set; }
        public int AmountToPay { get; set; }
    }
}
