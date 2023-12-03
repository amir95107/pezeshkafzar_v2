using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Pezeshkafzar_v2.Utilities
{
    public static class EntityFramworkExtension
    {

        public static IQueryable IncludeEntities<T>(this IQueryable<T> source,
params Expression<Func<T, object>>[] includes) where T : class, new()
        {
            return includes.Aggregate(source, (current, include) => current.Include(include));
        }
    }
}
