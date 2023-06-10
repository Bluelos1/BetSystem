using BetSystem.Contract;
using BetSystem.Model;
using FluentValidation;

namespace BetSystem.Validators
{
    public class EventResultValidator : AbstractValidator<EventResultDto>
    {
        public EventResultValidator()
        {
            RuleFor(x => x.BetOnResult).IsInEnum().WithMessage("Put 0-Win or 1-Lose or 2-Draw");
        }
    }

}
