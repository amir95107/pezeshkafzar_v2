using Microsoft.EntityFrameworkCore;

namespace Pezeshkafzar_v2.Repositories;
public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    DbSet<TEntity> Entities { get; }
    Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Modify(TEntity entity);

    Task SaveChangesAsync(CancellationToken cancellationToken);
    void SaveChanges();
}

