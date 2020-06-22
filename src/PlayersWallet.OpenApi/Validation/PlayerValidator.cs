using FluentValidation;
using PlayersWallet.Contracts.Entities;

namespace PlayersWallet.OpenApi.Validation
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty()
                .Length(1, 128).WithMessage("Player name must be between 1 and 128 chars (inclusive)");

            RuleFor(o => o.Email)
                .EmailAddress().WithMessage("Player email address must be valid one");
        }
    }
}