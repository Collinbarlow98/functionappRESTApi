using FunctionApp2.Heroes;
using FunctionApp2.QueryInterfaces;
using FunctionApp2.Searches;
using FunctionApp2.Sidekicks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Azure.Identity;
using FunctionApp2.Options;

[assembly: FunctionsStartup(typeof(ApiFunctionApp.Startup))]

namespace ApiFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            string appConfigUri = Environment.GetEnvironmentVariable("app_config_uri");
            var credentials = new DefaultAzureCredential();
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(appConfigUri), credentials)
                    .ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(credentials);
                    });
            });
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IHeroFunction, HeroFunction>();
            builder.Services.AddSingleton<ISidekickFunction, SidekickFunction>();
            builder.Services.AddSingleton<ISearchFunction, SearchFunction>();
            builder.Services.AddSingleton<IHeroSqlQueries, HeroSqlQueries>();
            builder.Services.AddSingleton<ISidekickSqlQueries, SidekickSqlQueries>();
            builder.Services.AddOptions<MyOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("FunctionApp").Bind(settings);
                });
        }
    }
}