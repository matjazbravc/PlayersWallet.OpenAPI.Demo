using FluentValidation;
using PlayersWallet.Contracts.Dto.Requests;

namespace PlayersWallet.OpenApi.Validation
{
    public class WinRequestValidator : AbstractValidator<WinRequest>
    {
        public WinRequestValidator()
        {
            RuleFor(req => req.PlayerId)
                .GreaterThan(0).WithMessage("Player Id must be greater than 0");

            RuleFor(req => req.Win)
                .GreaterThan(0).WithMessage("Win amount must be greater than 0 EUR");
        }
    }
}