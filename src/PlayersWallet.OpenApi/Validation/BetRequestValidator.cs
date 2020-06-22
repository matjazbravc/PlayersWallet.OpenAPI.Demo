using FluentValidation;
using PlayersWallet.Contracts.Dto.Requests;

namespace PlayersWallet.OpenApi.Validation
{
    public class BetRequestValidator : AbstractValidator<BetRequest>
    {
        public BetRequestValidator()
        {
            RuleFor(req => req.PlayerId)
                .GreaterThan(0).WithMessage("Player Id must be greater than 0");

            RuleFor(req => req.Bet)
                .GreaterThan(0).WithMessage("Bet amount must be greater than 0 EUR");
        }
    }
}