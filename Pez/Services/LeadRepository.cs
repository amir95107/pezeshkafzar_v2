using DataLayer.Data;
using DataLayer.Models;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class LeadRepository : BaseRepository<Lead_Clients, Guid>, ILeadRepository
    {
        public LeadRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }
    }
}
