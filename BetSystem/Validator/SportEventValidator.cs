using BetSystem.Contract;
using FluentValidation;

namespace BetSystem.Validator
{
    public class SportEventValidator : AbstractValidator<SportEventDto>
    {
        public SportEventValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
