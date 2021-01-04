using CarsIsland.MailSender.Infrastructure.Services.Integration;
using CarsIsland.MailSender.Infrastructure.Services.Integration.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarsIsland.MailSender.FuncApp.Core.DependencyInjection
{
    internal static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddCarReservationMessagingService(this IServiceCollection services)
        {
            services.AddScoped<ICarReservationMessagingService, CarReservationMessagingService>();
            return services;
        }
    }
}
