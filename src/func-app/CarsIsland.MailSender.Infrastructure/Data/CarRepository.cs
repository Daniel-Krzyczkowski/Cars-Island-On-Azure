using Azure.Cosmos;
using CarsIsland.MailSender.Core.Entities;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Logging;

namespace CarsIsland.MailSender.Infrastructure.Data
{
    public class CarRepository : CosmosDbDataRepository<Car>
    {
        public CarRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                         CosmosClient client,
                         ILogger<CarRepository> logger) : base(cosmosDbConfiguration, client, logger)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.CarContainerName;
    }
}
