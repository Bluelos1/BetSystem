using BetSystem.Contract;
using FluentValidation;

namespace BetSystem.Validator
{
    public class IdRequestValidator : AbstractValidator<IdRequestDto>
    {
        public IdRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Put valid Id");
        }

    }
}
