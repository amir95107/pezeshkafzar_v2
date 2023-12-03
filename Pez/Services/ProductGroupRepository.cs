using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class ProductGroupRepository : BaseRepository<Product_Groups, Guid>, IProductGroupRepository
    {
        public ProductGroupRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }

        public async Task<Product_Groups> FindByKeyAsync(int key)
            => await Entities
            .FirstOrDefaultAsync(x => x.UniqueKey == key && x.RemovedAt == null);
    }
}
