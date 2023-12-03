using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IFeatureRepository:IBaseRepository<Features,Guid>
    {
        Task<List<Features>> GetAllFeaturesAsync();
    }
}
