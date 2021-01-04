using Azure;
using Azure.Cosmos;
using CarsIsland.Core.Entities;
using CarsIsland.Core.Interfaces;
using CarsIsland.Infrastructure.Configuration.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsIsland.Infrastructure.Data
{
    public class CarReservationRepository : CosmosDbDataRepository<CarReservation>, ICarReservationRepository
    {
        public CarReservationRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                 CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.CarReservationContainerName;

        public async Task<CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom)
        {
            try
            {
                CosmosContainer container = GetContainer();
                var entities = new List<CarReservation>();
                QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.rentTo > @rentFrom AND c.carId = @carId")
                    .WithParameter("@rentFrom", rentFrom)
                    .WithParameter("@carId", carId);

                AsyncPageable<CarReservation> queryResultSetIterator = container.GetItemQueryIterator<CarReservation>(queryDefinition);

                await foreach (CarReservation carReservation in queryResultSetIterator)
                {
                    entities.Add(carReservation);
                }

                return entities.FirstOrDefault();
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {carId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }
    }
}
