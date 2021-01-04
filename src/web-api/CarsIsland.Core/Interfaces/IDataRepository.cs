using CarsIsland.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsIsland.Core.Interfaces
{
    public interface IDataRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(string entityId);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(string entityId);
        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
