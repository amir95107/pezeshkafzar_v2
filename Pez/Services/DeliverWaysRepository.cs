using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class DeliverWaysRepository : BaseRepository<DeliveryWays, Guid>, IDeliverWaysRepository
    {
        public DeliverWaysRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }

        public async Task<List<DeliveryWays>> GetAllExistingWays()
            => await Entities
            .Where(x=>x.RemovedAt == null && x.IsActive)
            .ToListAsync();

        public async Task<List<DeliveryWays>> GetAllWays()
            => await Entities
            .ToListAsync();
    }
}
