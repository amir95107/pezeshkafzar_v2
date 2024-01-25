using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class AddressRepository : BaseRepository<Addresses, Guid>, IAddressRepository
    {
        private readonly IQueryable<Addresses> NotRemoved;
        public AddressRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
        }

        public async Task<List<Addresses>> GetUserAddressesAsync(Guid userId)
            => await NotRemoved.Where(x => x.UserId == userId).ToListAsync();
    }
}
