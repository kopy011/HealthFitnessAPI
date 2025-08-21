using HealthFitnessAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Repository;

public interface IRepository<T> where T : AbstractEntity
{
    public IQueryable<T> GetAllAsQueryable(bool track = false);
    public Task<List<T>> GetAllAsync(bool track = false);
    public Task<T> GetByIdAsync(int id, bool track = false);
    public Task<T> CreateAsync(T entity);
    public Task CreateRangeAsync(IEnumerable<T> entities);
    public Task<T> UpdateAsync(T entity);
    public Task RemoveAsync(T entity, bool softDelete = false);
    public Task RemoveRangeAsync(List<T> entities);
}

public class Repository<T>(DbContext context) : IRepository<T>
    where T : AbstractEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> GetAllAsQueryable(bool track = false)
    {
        return track ? _dbSet.AsTracking() : _dbSet.AsQueryable().AsNoTracking();
    }

    public async Task<List<T>> GetAllAsync(bool track = false)
    {
        return track
            ? await _dbSet.AsQueryable().AsTracking().ToListAsync()
            : await _dbSet.AsQueryable().AsNoTracking().ToListAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        return (await _dbSet.AddAsync(entity)).Entity;
    }

    public async Task CreateRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        return (await Task.FromResult(_dbSet.Update(entity))).Entity;
    }

    public async Task<T> GetByIdAsync(int id, bool track = false)
    {
        return (track
                   ? await _dbSet.AsQueryable().AsTracking().FirstOrDefaultAsync(e => e.Id == id)
                   : await _dbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id))
               ?? throw new Exception("Entity not found");
    }

    public Task RemoveAsync(T entity, bool softDelete = false)
    {
        if (softDelete)
            entity.Deleted = true;
        else
            _dbSet.Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
}