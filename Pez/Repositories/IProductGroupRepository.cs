using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IProductGroupRepository:IBaseRepository<Product_Groups, Guid>
    {
        Task<Product_Groups> FindByKeyAsync(int key);
    }
}
