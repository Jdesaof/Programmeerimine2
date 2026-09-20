using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<T> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task SaveAsync(T entity, CancellationToken cancellationToken = default);
    }
}
