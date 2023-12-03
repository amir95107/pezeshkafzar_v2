using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class BrandRepository : BaseRepository<Brands, Guid>, IBrandRepository
    {
        private readonly IQueryable<Brands> NotRemoved;
        public BrandRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
        }

        public async Task<List<Brands>> GetAllBrandsAsync()
            => await NotRemoved.ToListAsync();
    }
}
