namespace CarsIsland.MailSender.Infrastructure.Services.Mail.Templates
{
    public class CarReservationConfirmationMailTemplate
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarImageUrl { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
