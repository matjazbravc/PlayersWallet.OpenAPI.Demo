using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlayersWallet.OpenApi.Data;
using PlayersWallet.OpenApi;
using PlayersWallet.Persistence.DbContexts;
using System.Linq;
using System;

namespace PlayersWallet.Tests.Services
{
    /// <summary>
    /// Customized WebApplicationFactory
    /// </summary>
    public class WebApiTestFactory : WebApplicationFactory<TestStartup>
    {
        public TService GetRequiredService<TService>()
        {
            if (Server == null)
            {
                CreateDefaultClient();
            }
            return Server.Host.Services.GetRequiredService<TService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(".") // Content root directory for web host
                .UseTestServer()
                .UseEnvironment("Test")
                .ConfigureTestServices(services =>
                {
                    // Remove the app's ApplicationDbContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add EntityFramework InMemoryDatabase
                    var serviceProvider = new ServiceCollection()
                      .AddEntityFrameworkInMemoryDatabase()
                      .BuildServiceProvider();

                    // Add ApplicationDbContext using an in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDatabaseTest");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    // Build the service provider
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context (ApplicationDbContext)
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
                        var logger = scopedServices.GetRequiredService<ILogger<WebApiTestFactory>>();

                        // Ensure the database is created
                        dbContext.Database.EnsureCreated();

                        try
                        {
                            // Seed the database with test data
                            SeedData.Initialize(dbContext);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                        }
                    }
                });

            // Call base Configuration
            base.ConfigureWebHost(builder);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, b) =>
                {
                    context.HostingEnvironment.ApplicationName = typeof(Program).Assembly.GetName().Name;
                })
                .UseStartup<TestStartup>();
            return hostBuilder;
        }
    }
}