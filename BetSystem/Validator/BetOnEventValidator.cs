using BetSystem.Contract;
using BetSystem.Model;
using FluentValidation;

namespace BetSystem.Validators
{
    public class BetOnEventValidator : AbstractValidator<BetOnEventDto>
    {
        public BetOnEventValidator()
        {
            
            RuleFor(x => x.BetOnResult).IsInEnum().WithMessage("Put 0-Win or 1-Lose or 2-Draw");
            RuleFor(x => x.Amount).NotNull();
            RuleFor(x => x.Interest).NotNull();
            RuleFor(x => x.AmountToPay).NotNull();
        }
    }
}
