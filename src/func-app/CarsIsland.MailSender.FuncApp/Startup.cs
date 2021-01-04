using CarsIsland.MailSender.FuncApp.Core.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(CarsIsland.MailSender.FuncApp.Startup))]
namespace CarsIsland.MailSender.FuncApp
{
    internal class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureSettings();
            builder.Services.AddAppConfiguration(_configuration);
            builder.Services.AddMailDeliveryServices();
            builder.Services.AddDataServices();
            builder.Services.AddUserManagementServices();
            builder.Services.AddCarReservationMessagingService();
        }

        private void ConfigureSettings()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            _configuration = config;
        }
    }
}
