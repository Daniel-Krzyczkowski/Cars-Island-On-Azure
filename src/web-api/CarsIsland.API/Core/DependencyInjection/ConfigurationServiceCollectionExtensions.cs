using CarsIsland.Infrastructure.Configuration;
using CarsIsland.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CarsIsland.API.Core.DependencyInjection
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BlobStorageServiceConfiguration>(config.GetSection("BlobStorageSettings"));
            services.AddSingleton<IValidateOptions<BlobStorageServiceConfiguration>, BlobStorageServiceConfigurationValidation>();
            var blobStorageServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<BlobStorageServiceConfiguration>>().Value;
            services.AddSingleton<IBlobStorageServiceConfiguration>(blobStorageServiceConfiguration);

            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));
            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
            var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
            services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);


            services.Configure<MessagingServiceConfiguration>(config.GetSection("ServiceBusSettings"));
            services.AddSingleton<IValidateOptions<MessagingServiceConfiguration>, MessagingServiceConfigurationValidation>();
            var messagingServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MessagingServiceConfiguration>>().Value;
            services.AddSingleton<IMessagingServiceConfiguration>(messagingServiceConfiguration);

            return services;
        }
    }
}
