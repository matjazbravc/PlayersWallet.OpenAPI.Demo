using FluentValidation;
using PlayersWallet.Contracts.Dto.Requests;

namespace PlayersWallet.OpenApi.Validation
{
    public class PayInRequestValidator : AbstractValidator<PayInRequest>
    {
        public PayInRequestValidator()
        {
            RuleFor(req => req.PlayerId)
                .GreaterThan(0).WithMessage("Player Id must be greater than 0");

            RuleFor(req => req.PayIn)
                .GreaterThan(0).WithMessage("PayIn amount must be greater than 0 EUR");
        }
    }
}