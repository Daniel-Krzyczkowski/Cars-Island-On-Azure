using CarsIsland.MailSender.Infrastructure.Services.Mail.Templates;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.Mail.Interfaces
{
    public interface IMailDeliveryService
    {
        Task SendInvitationMessageAsync(CarReservationConfirmationMailTemplate carReservationConfirmationMailTemplate);
    }
}
