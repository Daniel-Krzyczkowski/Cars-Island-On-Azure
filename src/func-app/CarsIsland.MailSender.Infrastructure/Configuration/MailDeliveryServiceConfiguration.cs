using CarsIsland.MailSender.Infrastructure.Configuration.Exceptions;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace CarsIsland.MailSender.Infrastructure.Configuration
{
    public class MailDeliveryServiceConfiguration : IMailDeliveryServiceConfiguration
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string CarReservationConfirmationTemplateId { get; set; }
    }

    public class MailDeliveryServiceConfigurationValidation : IValidateOptions<MailDeliveryServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MailDeliveryServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                throw new AppConfigurationException($"{nameof(options.ApiKey)} configuration parameter for the SendGrid Service is required");
            }

            if (string.IsNullOrEmpty(options.FromEmail))
            {
                throw new AppConfigurationException($"{nameof(options.FromEmail)} configuration parameter for the SendGrid Service is required");
            }

            if (string.IsNullOrEmpty(options.CarReservationConfirmationTemplateId))
            {
                throw new AppConfigurationException($"{nameof(options.CarReservationConfirmationTemplateId)} configuration parameter for the SendGrid Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
