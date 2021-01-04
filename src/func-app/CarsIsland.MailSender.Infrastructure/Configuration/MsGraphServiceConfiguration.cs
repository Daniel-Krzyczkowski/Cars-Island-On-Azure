using CarsIsland.MailSender.Infrastructure.Configuration.Exceptions;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace CarsIsland.MailSender.Infrastructure.Configuration
{
    public class MsGraphServiceConfiguration : IMsGraphServiceConfiguration
    {
        public string TenantId { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }

    public class MsGraphServiceConfigurationValidation : IValidateOptions<MsGraphServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MsGraphServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.TenantId))
            {
                throw new AppConfigurationException($"{nameof(options.TenantId)} configuration parameter for the Microsoft Graph Service is required");
            }

            if (string.IsNullOrEmpty(options.AppId))
            {
                throw new AppConfigurationException($"{nameof(options.AppId)} configuration parameter for the Microsoft Graph Service is required");
            }

            if (string.IsNullOrEmpty(options.AppSecret))
            {
                throw new AppConfigurationException($"{nameof(options.AppSecret)} configuration parameter for the Microsoft Graph Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
