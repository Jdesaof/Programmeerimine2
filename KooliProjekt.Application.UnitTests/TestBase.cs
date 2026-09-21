using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.UnitTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;
        private bool disposedValue;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                _dbContext = new ApplicationDbContext(options);
                return _dbContext;
            }
        }

        // 22.01: a context with no provider exposes accidental database access.
        protected ApplicationDbContext GetFaultyDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            return new ApplicationDbContext(options.Options);
        }

        protected DeleteSaveFailingDbContext GetDeleteSaveFailingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new DeleteSaveFailingDbContext(options);
        }

        protected sealed class DeleteSaveFailingDbContext : ApplicationDbContext
        {
            public bool FailOnSave { get; set; }
            public InvalidOperationException SaveFailure { get; } =
                new InvalidOperationException("Simulated save failure.");

            public DeleteSaveFailingDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options) { }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                return FailOnSave ? Task.FromException<int>(SaveFailure) :
                    base.SaveChangesAsync(cancellationToken);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
