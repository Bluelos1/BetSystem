using BetSystem.Model;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace BetSystem.Contract
{
    public record BetOnEventDto
    {
        public int Id { get; set; }
        public BetResult BetOnResult { get; set; }
        public int Amount { get; set; }
        public int Interest { get; set; }
        public int AmountToPay { get; set; }
        public int TeamId { get; set; }
        public int EventId { get; set; }
    }
}
