namespace CarsIsland.MailSender.Infrastructure.Configuration.Interfaces
{
    public interface IMailDeliveryServiceConfiguration
    {
        string ApiKey { get; set; }
        string FromEmail { get; set; }
        string CarReservationConfirmationTemplateId { get; set; }
    }
}
