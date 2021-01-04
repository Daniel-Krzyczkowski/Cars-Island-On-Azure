namespace CarsIsland.MailSender.Core.Entities
{
    public class UserAccount : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
