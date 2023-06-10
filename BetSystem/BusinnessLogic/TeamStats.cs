namespace BetSystem.Contract.BusinnessLogic
{
    public record TeamStats
    {
        public string TeamName { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int DrawCount { get; set; }
    }

}
