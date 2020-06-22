
using FluentValidation;
using PlayersWallet.Contracts.Entities;

namespace PlayersWallet.OpenApi.Validation
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(req => req.Bet)
               .GreaterThan(0).WithMessage("Bet amount must be greater than 0 EUR");

            RuleFor(tr => tr.PayIn)
                .GreaterThan(0).WithMessage("PayIn amount must be greater than 0 EUR");

            RuleFor(req => req.Win)
                .GreaterThan(0).WithMessage("Win amount must be greater than 0 EUR");
        }
    }
}