using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using PlayersWallet.OpenApi.Contracts;
using System.IO;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;

namespace PlayersWallet.OpenApi.Installers
{
    internal class RegisterSwagger : IServiceRegistration
    {
        public void Register(IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = $"Player's Wallet OpenAPI Demo",
                    Version = "v1.0",
                    Description = "Current API Version",
                    Contact = new OpenApiContact
                    {
                        Name = "Casino Adventures Corp.",
                        Email = "casino.adventures.support@casino-adventures.com",
                        Url = new Uri("https://matjazbravc.github.io/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICENSE...",
                        Url = new Uri("https://demo.com/license")
                    }
                });
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor.GetApiVersionModel();
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    return actionApiVersionModel.DeclaredApiVersions.Count > 0 ? actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v}" == docName) : actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v}" == docName);
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlDocFile = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, xmlFile);
                options.IncludeXmlComments(xmlDocFile);
                options.DescribeAllParametersInCamelCase();
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }
    }
}
