using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayersWallet.OpenApi.Contracts;
using System;
using System.Linq;

namespace PlayersWallet.OpenApi.Extensions
{
    internal static class ServiceRegistrationExtension
    {
        public static void AddServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var appServices = typeof(Startup).Assembly.DefinedTypes
                            .Where(x => typeof(IServiceRegistration)
                            .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                            .Select(Activator.CreateInstance)
                            .Cast<IServiceRegistration>().ToList();
            appServices.ForEach(svc => svc.Register(services, configuration));
        }
    }
}
