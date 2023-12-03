using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IHomeRepository : IBaseRepository<Page,Guid>
    {
        Task<Page> GetPageDetailAsync(int pageKey);
        Task<List<Slider>> GetSliderListAsync( );
        Task<List<ContactForm>> GetFaqsAsync( );
    }
}