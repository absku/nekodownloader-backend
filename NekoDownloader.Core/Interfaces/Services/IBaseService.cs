namespace NekoDownloader.Core.Interfaces.Services;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<TEntity> GetByUuid(Guid uuid);
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Create(TEntity newEntity);
    Task<TEntity> Update(Guid entityToBeUpdatedId, TEntity newEntityValues);
    Task Delete(Guid uuid);
}