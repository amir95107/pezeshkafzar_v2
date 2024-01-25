using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IAddressRepository : IBaseRepository<Addresses, Guid>
    {
        Task<List<Addresses>> GetUserAddressesAsync(Guid userId);
    }
}
