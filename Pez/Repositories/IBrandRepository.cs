using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IBrandRepository : IBaseRepository<Brands, Guid>
    {
        Task<List<Brands>> GetAllBrandsAsync();
    }
}
