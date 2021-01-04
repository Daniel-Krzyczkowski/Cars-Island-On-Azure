using Azure.Storage.Blobs;
using CarsIsland.Infrastructure.Configuration.Interfaces;
using CarsIsland.Infrastructure.Services.Storage;
using CarsIsland.Infrastructure.Services.Storage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarsIsland.API.Core.DependencyInjection
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var storageConfiguration = serviceProvider.GetRequiredService<IBlobStorageServiceConfiguration>();

            services.AddSingleton(factory => new BlobServiceClient(storageConfiguration.ConnectionString));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            return services;
        }
    }
}
