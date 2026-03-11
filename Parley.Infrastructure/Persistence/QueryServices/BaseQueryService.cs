using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Parley.Application.Contracts.Query;

namespace Parley.Infrastructure.Persistence.QueryServices;

public class BaseQueryService<TKey, TEntity>:IQueryService<TKey,TEntity> where  TEntity:class
{
    private readonly ParleyDbContext _dbContext;

    public BaseQueryService(ParleyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync([id],cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().Where(predicate)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return  _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate,cancellationToken);
    }

    public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity=await _dbContext.Set<TEntity>().FindAsync([id],cancellationToken);
        return entity != null;
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().AnyAsync(predicate,cancellationToken: cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().CountAsync(cancellationToken: cancellationToken);
        
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken: cancellationToken);
    }
}