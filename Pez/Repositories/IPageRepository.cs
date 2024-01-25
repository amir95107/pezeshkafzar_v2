using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IPageRepository:IBaseRepository<Page,Guid>
    {
        Task<List<Page>> GetAllAsync();
    }
}
