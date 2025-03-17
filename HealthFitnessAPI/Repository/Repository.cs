using HealthFitnessAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace HealthFitnessAPI.Repository
{
    public interface IRepository<T> where T : AbstractEntity
    {
        public Task<List<T>> GetAllAsync(bool track = false);
        public Task<T> GetByIdAsync(int id, bool track = false);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
    }

    public class Repository<T>(DbContext context) : IRepository<T>
        where T : AbstractEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<List<T>> GetAllAsync(bool track = false)
        {
            return track ? await _dbSet.AsQueryable().AsTracking().ToListAsync() : await _dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return (await Task.FromResult(_dbSet.Update(entity))).Entity;
        }

        public async Task<T> GetByIdAsync(int id, bool track = false)
        {
            return (track ? await _dbSet.AsQueryable().AsTracking().FirstOrDefaultAsync(e => e.Id == id) : await _dbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id))
                ?? throw new Exception("Entity not found");
        }
    }
}
