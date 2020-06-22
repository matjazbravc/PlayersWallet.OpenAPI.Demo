using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlayersWallet.OpenApi.Contracts
{
    public interface IServiceRegistration
    {
        void Register(IServiceCollection services, IConfiguration configuration);
    }
}
