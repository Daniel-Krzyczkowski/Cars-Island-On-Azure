using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.Mail.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.Mail.Templates;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.Mail
{
    public class MailDeliveryService : IMailDeliveryService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IMailDeliveryServiceConfiguration _mailDeliveryServiceConfiguration;
        private readonly ILogger<MailDeliveryService> _logger;

        public MailDeliveryService(ISendGridClient sendGridClient,
                                   IMailDeliveryServiceConfiguration mailDeliveryServiceConfiguration,
                                    ILogger<MailDeliveryService> logger)
        {
            _sendGridClient = sendGridClient
                 ?? throw new ArgumentNullException(nameof(sendGridClient));

            _mailDeliveryServiceConfiguration = mailDeliveryServiceConfiguration
                 ?? throw new ArgumentNullException(nameof(mailDeliveryServiceConfiguration));

            _logger = logger
                 ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendInvitationMessageAsync(CarReservationConfirmationMailTemplate carReservationConfirmationMailTemplate)
        {
            var emailMessage = MailHelper.CreateSingleTemplateEmail(
                 new EmailAddress(_mailDeliveryServiceConfiguration.FromEmail),
                 new EmailAddress(carReservationConfirmationMailTemplate.CustomerEmail),
                 _mailDeliveryServiceConfiguration.CarReservationConfirmationTemplateId,
                 new
                 {
                     customerName = carReservationConfirmationMailTemplate.CustomerName,
                     carBrand = carReservationConfirmationMailTemplate.CarBrand,
                     carModel = carReservationConfirmationMailTemplate.CarModel,
                     carImageUrl = carReservationConfirmationMailTemplate.CarImageUrl,
                     fromDate = carReservationConfirmationMailTemplate.FromDate,
                     toDate = carReservationConfirmationMailTemplate.ToDate
                 });

            var response = await _sendGridClient.SendEmailAsync(emailMessage);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var responseContent = await response.Body.ReadAsStringAsync();
                _logger.LogError($"SendGrid service returned status code {response.StatusCode} with response: {responseContent}");
            }
        }
    }
}
