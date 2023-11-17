using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly ApplicationDBContext Context;
        public DbSet<TEntity> Entities { get; }
        public BaseRepository(IHttpContextAccessor accessor, ApplicationDBContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }

        public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken)
        => await Entities.FindAsync(new Guid(id.ToString()));

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Entities.AddAsync(entity, cancellationToken);
        }

        public void Modify(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
            => await Context.SaveChangesAsync();

        public void SaveChanges()
            => Context.SaveChanges();
    }
}
