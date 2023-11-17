using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IHomeRepository : IBaseRepository<Page,Guid>
    {
        Task<Page> GetPageDetailAsync(int pageKey, CancellationToken cancellationToken);
        Task<List<Slider>> GetSliderListAsync(CancellationToken cancellationToken);
        Task<List<ContactForm>> GetFaqsAsync(CancellationToken cancellationToken);
    }
}