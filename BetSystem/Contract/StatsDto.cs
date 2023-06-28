namespace BetSystem.Contract
{
    public record StatsDto
    {
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int DrawCount { get; set; }
    }
}
