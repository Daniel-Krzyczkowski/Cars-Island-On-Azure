using CarsIsland.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace CarsIsland.Infrastructure.Configuration
{
    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CarContainerName { get; set; }
        public string CarContainerPartitionKeyPath { get; set; }
        public string EnquiryContainerName { get; set; }
        public string EnquiryContainerPartitionKeyPath { get; set; }
        public string CarReservationContainerName { get; set; }
        public string CarReservationPartitionKeyPath { get; set; }
    }

    public class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
    {
        public ValidateOptionsResult Validate(string name, CosmosDbConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.CarContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.EnquiryContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.CarReservationContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarReservationContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.DatabaseName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.DatabaseName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.CarContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.EnquiryContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.CarReservationPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarReservationPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
