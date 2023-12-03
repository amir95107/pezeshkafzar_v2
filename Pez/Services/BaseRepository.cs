﻿using DataLayer.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;
using System.Linq.Expressions;

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

        public async Task<TEntity> FindAsync(TKey id)
        => await Entities.FindAsync(new Guid(id.ToString()));


        public async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public void Modify(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task SaveChangesAsync()
            => await Context.SaveChangesAsync();

        public void SaveChanges()
            => Context.SaveChanges();

        public async Task Remove(TKey id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity is null)
                throw new Exception($"Entity {typeof(TEntity)} not found!");
            Entities.Remove(entity);
        }

        public void Remove(TEntity entity)
            => Entities.Remove(entity);

    }
}
