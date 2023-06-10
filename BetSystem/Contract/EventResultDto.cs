using BetSystem.Model;

namespace BetSystem.Contract
{
    public record EventResultDto
    {
        public int Id { get; set; }
        public BetResult BetOnResult { get; set; }
    }
}
