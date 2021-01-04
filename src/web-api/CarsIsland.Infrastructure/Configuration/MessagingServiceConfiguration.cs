using CarsIsland.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace CarsIsland.Infrastructure.Configuration
{
    public class MessagingServiceConfiguration : IMessagingServiceConfiguration
    {
        public string QueueName { get; set; }
        public string ListenAndSendConnectionString { get; set; }
    }

    public class MessagingServiceConfigurationValidation : IValidateOptions<MessagingServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MessagingServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ListenAndSendConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ListenAndSendConnectionString)} configuration parameter for the Azure Service Bus is required");
            }

            if (string.IsNullOrEmpty(options.QueueName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.QueueName)} configuration parameter for the Azure Service Bus is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
