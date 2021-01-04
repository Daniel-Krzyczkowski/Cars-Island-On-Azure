namespace CarsIsland.MailSender.Infrastructure.Configuration.Interfaces
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CarContainerName { get; set; }
        string CarContainerPartitionKeyPath { get; set; }
    }
}
