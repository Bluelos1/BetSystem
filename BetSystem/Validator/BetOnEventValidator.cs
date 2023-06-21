﻿using BetSystem.Contract;
using FluentValidation;

namespace BetSystem.Validators
{
    public class BetOnEventValidator : AbstractValidator<BetOnEventDto>
    {
        public BetOnEventValidator()
        {

            RuleFor(x => x.BetOnResult).IsInEnum().WithMessage("Put 0-Win or 1-Lose or 2-Draw");
            RuleFor(x => x.Amount).NotNull().NotEmpty();
            RuleFor(x => x.Interest).NotNull().NotEmpty();
            RuleFor(x => x.AmountToPay).NotNull().NotEmpty();
            RuleFor(X => X.TeamId).NotNull().NotEmpty();
            RuleFor(X => X.EventId).NotNull().NotEmpty();

        }
    }
}
