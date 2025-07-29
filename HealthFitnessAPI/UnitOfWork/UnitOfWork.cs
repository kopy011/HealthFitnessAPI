using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Repository;
using Microsoft.EntityFrameworkCore;
namespace HealthFitnessAPI.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity;
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : AbstractEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbContext GetDbContext();
    }

    public class UnitOfWork<TContext>(TContext context) : IUnitOfWork
        where TContext : DbContext
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity
        {
            var type = typeof(TEntity);
            if (_repositories.TryGetValue(type, out var value))
                return (IRepository<TEntity>)value;
            value = new Repository<TEntity>(context);
            _repositories[type] = value;

            return (IRepository<TEntity>)value;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : AbstractEntity
        {
            return context.Set<TEntity>();
        }

        public int SaveChanges()
        {
            var count = context.SaveChanges();
            return count;
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public DbContext GetDbContext()
        {
            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories.Clear();
                    context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
