using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class FeatureRepository : BaseRepository<Features, Guid>, IFeatureRepository
    {
        private readonly IQueryable<Features> NotRemoved;
        public FeatureRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
        }

        public async Task<List<Features>> GetAllFeaturesAsync()
            => await NotRemoved.ToListAsync();
    }
}
