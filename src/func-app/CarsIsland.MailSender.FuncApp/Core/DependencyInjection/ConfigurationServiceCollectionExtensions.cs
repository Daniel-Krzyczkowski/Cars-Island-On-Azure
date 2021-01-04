using CarsIsland.MailSender.Infrastructure.Configuration;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CarsIsland.MailSender.FuncApp.Core.DependencyInjection
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MsGraphServiceConfiguration>(config.GetSection("MsGraphServiceConfiguration"));
            services.AddSingleton<IValidateOptions<MsGraphServiceConfiguration>, MsGraphServiceConfigurationValidation>();
            var msGraphServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MsGraphServiceConfiguration>>().Value;
            services.AddSingleton<IMsGraphServiceConfiguration>(msGraphServiceConfiguration);

            services.Configure<MailDeliveryServiceConfiguration>(config.GetSection("MailDeliveryServiceConfiguration"));
            services.AddSingleton<IValidateOptions<MailDeliveryServiceConfiguration>, MailDeliveryServiceConfigurationValidation>();
            var mailDeliveryServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MailDeliveryServiceConfiguration>>().Value;
            services.AddSingleton<IMailDeliveryServiceConfiguration>(mailDeliveryServiceConfiguration);

            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));
            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
            var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
            services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);

            return services;
        }
    }
}
