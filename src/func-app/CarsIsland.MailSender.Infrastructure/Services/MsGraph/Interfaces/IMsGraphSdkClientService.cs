using CarsIsland.MailSender.Core.Entities;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.MsGraph.Interfaces
{
    public interface IMsGraphSdkClientService
    {
        Task<UserAccount> GetUserAsync(string userId);
    }
}
