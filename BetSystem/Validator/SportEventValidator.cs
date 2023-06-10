using BetSystem.Contract;
using BetSystem.Endpoint;
using FluentValidation;

namespace BetSystem.Validator
{
    public class SportEventValidator : AbstractValidator<SportEventDto>
    {
        public SportEventValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
        }
    }
}
