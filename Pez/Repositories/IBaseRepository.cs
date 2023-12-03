using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Pezeshkafzar_v2.Repositories;
public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    DbSet<TEntity> Entities { get; }
    Task<TEntity> FindAsync(TKey id);
    Task AddAsync(TEntity entity);
    void Modify(TEntity entity);
    Task Remove(TKey id);
    void Remove(TEntity entity);

    Task SaveChangesAsync();
    void SaveChanges();
}

