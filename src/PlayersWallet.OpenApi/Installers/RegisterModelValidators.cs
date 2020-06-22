using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayersWallet.Contracts.Dto.Requests;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.OpenApi.Contracts;
using PlayersWallet.OpenApi.Validation;

namespace PlayersWallet.OpenApi.Installers
{
    internal class RegisterModelValidators : IServiceRegistration
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            // Register Model Validators
            services.AddTransient<IValidator<BetRequest>, BetRequestValidator>();
            services.AddTransient<IValidator<PayInRequest>, PayInRequestValidator>();
            services.AddTransient<IValidator<Player>, PlayerValidator>();
            services.AddTransient<IValidator<Transaction>, TransactionValidator>();
            services.AddTransient<IValidator<WinRequest>, WinRequestValidator>();

            // Disable Automatic Model State Validation built-in to ASP.NET Core
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
