using CarsIsland.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace CarsIsland.Infrastructure.Configuration
{
    public class BlobStorageServiceConfiguration : IBlobStorageServiceConfiguration
    {
        public string ContainerName { get; set; }
        public string ConnectionString { get; set; }
        public string Key { get; set; }
        public string AccountName { get; set; }
    }

    public class BlobStorageServiceConfigurationValidation : IValidateOptions<BlobStorageServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, BlobStorageServiceConfiguration options)
        {

            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrEmpty(options.ContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ContainerName)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrEmpty(options.Key))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Key)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrEmpty(options.AccountName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.AccountName)} configuration parameter for the Azure Storage Account is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
