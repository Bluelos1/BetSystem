using BetSystem.Model;

namespace BetSystem.Contract
{
    public record BetOnEventDto
    {
        public int Id { get; set; }
        public BetResult BetOnResult { get; set; }
        public int Amount { get; set; }
        public int Interest { get; set; }
        public int AmountToPay { get; set; }
    }
}
