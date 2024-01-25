using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IDeliverWaysRepository:IBaseRepository<DeliveryWays,Guid>
    {
        Task<List<DeliveryWays>> GetAllWays();
        Task<List<DeliveryWays>> GetAllExistingWays();
    }
}
