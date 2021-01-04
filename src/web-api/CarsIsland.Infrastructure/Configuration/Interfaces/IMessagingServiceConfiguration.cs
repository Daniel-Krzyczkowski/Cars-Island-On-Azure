namespace CarsIsland.Infrastructure.Configuration.Interfaces
{
    public interface IMessagingServiceConfiguration
    {
        string ListenAndSendConnectionString { get; set; }
        string QueueName { get; set; }
    }
}
