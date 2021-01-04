using CarsIsland.MailSender.Core.Entities;
using CarsIsland.MailSender.Infrastructure.Configuration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.MsGraph.Interfaces;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.MsGraph
{
    public class MsGraphSdkClientService : IMsGraphSdkClientService
    {
        private readonly IMsGraphServiceConfiguration _msGraphServiceConfiguration;
        private readonly IGraphServiceClient _graphServiceClient;

        public MsGraphSdkClientService(IMsGraphServiceConfiguration msGraphServiceConfiguration,
                                       IGraphServiceClient graphServiceClient)
        {
            _msGraphServiceConfiguration = msGraphServiceConfiguration
                 ?? throw new ArgumentNullException(nameof(msGraphServiceConfiguration));

            _graphServiceClient = graphServiceClient
                 ?? throw new ArgumentNullException(nameof(graphServiceClient));
        }

        public async Task<UserAccount> GetUserAsync(string userId)
        {
            var user = await _graphServiceClient.Users[userId]
                             .Request()
                             .Select(e => new
                             {
                                 e.Id,
                                 e.GivenName,
                                 e.Surname,
                                 e.Identities
                             })
                             .GetAsync();

            if (user != null)
            {
                var email = user.Identities.ToList()
                            .FirstOrDefault(i => i.SignInType == "emailAddress")
                            ?.IssuerAssignedId;

                return new UserAccount
                {
                    Id = user.Id,
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    Email = email
                };
            }

            else
            {
                return null;
            }
        }
    }
}
