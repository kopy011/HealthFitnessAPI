using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;

namespace HealthFitnessAPI.Services;

public interface IAbstractService<TEntity> where TEntity : AbstractEntity
{
    Task<TEntity> Create(TEntity entity);
    Task<List<TEntity>> GetAll(bool track = false);
    Task<TEntity> GetById(int id, bool track = false);
    Task<TEntity> Update(TEntity entity);
    Task DeleteSoft(int id);
}

public class AbstractService<TEntity>(IUnitOfWork unitOfWork) : IAbstractService<TEntity> where TEntity : AbstractEntity
{
    public virtual async Task<TEntity> Create(TEntity entity)
    {
        var result = await unitOfWork.GetRepository<TEntity>().CreateAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return result;
    }

    public virtual async Task<List<TEntity>> GetAll(bool track = false)
    {
        return await unitOfWork.GetRepository<TEntity>().GetAllAsync(track);
    }

    public async Task<TEntity> GetById(int id, bool track = false)
    {
        return await unitOfWork.GetRepository<TEntity>().GetByIdAsync(id, track);
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        var result = await unitOfWork.GetRepository<TEntity>().UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task DeleteSoft(int id)
    {
        var entity = await GetById(id, true);
        entity.Deleted = true;
        await unitOfWork.SaveChangesAsync();
    }
}