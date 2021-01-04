using Azure;
using Azure.Cosmos;
using CarsIsland.MailSender.Core.Entities;
using CarsIsland.MailSender.Core.Interfaces;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Data
{
    public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;
        private readonly ILogger<CosmosDbDataRepository<T>> _logger;

        public abstract string ContainerName { get; }

        public CosmosDbDataRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                           CosmosClient client,
                            ILogger<CosmosDbDataRepository<T>> logger)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));

            _logger = logger
                    ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex.Message);

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task DeleteAsync(string entityId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                await container.DeleteItemAsync<T>(entityId, new PartitionKey(entityId));
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex.Message);

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex.Message);

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<BaseEntity> entityResult = await container
                                                           .ReadItemAsync<BaseEntity>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));

                if (entityResult != null)
                {
                    await container
                          .ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
                }
                return entity;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex.Message);

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                CosmosContainer container = GetContainer();
                AsyncPageable<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
                List<T> entities = new List<T>();

                await foreach (var entity in queryResultSetIterator)
                {
                    entities.Add(entity);
                }

                return entities;

            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex.Message);

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }


        protected CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            var container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
