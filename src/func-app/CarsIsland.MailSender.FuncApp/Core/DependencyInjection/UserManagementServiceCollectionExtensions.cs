using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.MsGraph;
using CarsIsland.MailSender.Infrastructure.Services.MsGraph.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace CarsIsland.MailSender.FuncApp.Core.DependencyInjection
{
    internal static class UserManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IGraphServiceClient>(implementationFactory =>
            {
                var msGraphServiceConfiguration = implementationFactory.GetRequiredService<IMsGraphServiceConfiguration>();
                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(msGraphServiceConfiguration.AppId)
                    .WithTenantId(msGraphServiceConfiguration.TenantId)
                    .WithClientSecret(msGraphServiceConfiguration.AppSecret)
                    .Build();

                ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
                return new GraphServiceClient(authProvider);
            });

            services.AddScoped<IMsGraphSdkClientService, MsGraphSdkClientService>();

            return services;
        }
    }
}
