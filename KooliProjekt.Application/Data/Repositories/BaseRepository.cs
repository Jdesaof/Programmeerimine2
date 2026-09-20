using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext Context;
        protected BaseRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual Task<T> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) return Task.FromResult<T>(null);
            return Context.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) return Task.FromResult(false);
            return Context.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task SaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            if (entity.Id < 0) throw new ArgumentException("Id cannot be negative.", nameof(entity));
            if (entity.Id == 0) Context.Set<T>().Add(entity);
            else Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
