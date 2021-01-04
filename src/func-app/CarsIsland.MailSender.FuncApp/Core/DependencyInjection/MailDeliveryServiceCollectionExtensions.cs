using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.Mail;
using CarsIsland.MailSender.Infrastructure.Services.Mail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace CarsIsland.MailSender.FuncApp.Core.DependencyInjection
{
    internal static class MailDeliveryServiceCollectionExtensions
    {
        public static IServiceCollection AddMailDeliveryServices(this IServiceCollection services)
        {
            services.AddSendGrid((sp, options) =>
            {
                var mailDeliveryServiceConfiguration = sp.GetRequiredService<IMailDeliveryServiceConfiguration>();
                options.ApiKey = mailDeliveryServiceConfiguration.ApiKey;
            });

            services.AddScoped<IMailDeliveryService, MailDeliveryService>();
            return services;
        }
    }
}
