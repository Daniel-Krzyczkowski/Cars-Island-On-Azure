using Azure.Cosmos;
using CarsIsland.Core.Entities;
using CarsIsland.Infrastructure.Configuration.Interfaces;

namespace CarsIsland.Infrastructure.Data
{
    public class EnquiryRepository : CosmosDbDataRepository<Enquiry>
    {
        public EnquiryRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                         CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.EnquiryContainerName;
    }
}
