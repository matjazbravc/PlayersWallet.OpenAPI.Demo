using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayersWallet.Persistence.Repositories;
using PlayersWallet.OpenApi.Contracts;

namespace PlayersWallet.OpenApi.Installers
{
    internal class RegisterRepositories : IServiceRegistration
    {
        public void Register(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
