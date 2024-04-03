using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Infrastructure.Data;

namespace NekoDownloader.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    internal AppDbContext Context;
    internal readonly DbSet<TEntity> DbSet;
    private readonly char[] _separator = new char[] { ',' };

    public BaseRepository(AppDbContext context)
    {
        this.Context = context;
        this.DbSet = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
            query = query.Where(filter);

        query = includeProperties.Split(_separator, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetByUuidAsync(Guid uuid, string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;
        query = includeProperties.Split(_separator, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        var result = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Uuid") == uuid);
        if (result == null)
            throw new InvalidOperationException();
        return result;
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.SingleOrDefaultAsync(predicate) ?? throw new InvalidOperationException();
    }

    public virtual Task Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
    {
        var toUpdate = entitiesToUpdate as TEntity[] ?? entitiesToUpdate.ToArray();
        DbSet.AttachRange(toUpdate);
        Context.Entry(toUpdate).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
