using Azure.Cosmos;
using CarsIsland.MailSender.Core.Entities;
using CarsIsland.MailSender.Core.Interfaces;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CarsIsland.MailSender.FuncApp.Core.DependencyInjection
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var cosmoDbConfiguration = serviceProvider.GetRequiredService<ICosmosDbConfiguration>();
            CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString);
            CosmosDatabase database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                                   .GetAwaiter()
                                                   .GetResult();

            database.CreateContainerIfNotExistsAsync(
                cosmoDbConfiguration.CarContainerName,
                cosmoDbConfiguration.CarContainerPartitionKeyPath,
                400)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(cosmosClient);

            services.AddSingleton<IDataRepository<Car>, CarRepository>();

            return services;
        }
    }
}
