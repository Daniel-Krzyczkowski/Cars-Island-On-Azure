using System;

namespace CarsIsland.Core.Interfaces
{
    public interface IIdentityService
    {
        Guid GetUserIdentity();
        string GetUserEmail();
    }
}
