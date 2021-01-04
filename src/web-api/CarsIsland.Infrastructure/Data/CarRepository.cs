using Azure.Cosmos;
using CarsIsland.Core.Entities;
using CarsIsland.Infrastructure.Configuration.Interfaces;

namespace CarsIsland.Infrastructure.Data
{
    public class CarRepository : CosmosDbDataRepository<Car>
    {
        public CarRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                         CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.CarContainerName;
    }
}
