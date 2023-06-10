using BetSystem.Contract;
using BetSystem.Model;
using FluentValidation;

namespace BetSystem.Validators
{
    public class TeamValidator : AbstractValidator<TeamDto>
    {
        public TeamValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
