using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class PageRepository : BaseRepository<Page, Guid>, IPageRepository
    {
        public PageRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }

        public async Task<List<Page>> GetAllAsync()
            => await Entities
            .ToListAsync();
    }
}
