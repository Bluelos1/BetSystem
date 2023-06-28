using BetSystem.Contract;
using FluentValidation;

namespace BetSystem.Validators
{
    public class EventResultValidator : AbstractValidator<EventResultDto>
    {
        public EventResultValidator()
        {
            RuleFor(x => x.BetOnResult).IsInEnum().WithMessage("Put 0-Win or 1-Lose or 2-Draw");
            RuleFor(x => x.TeamId).NotNull().NotEmpty();
        }
    }

}
