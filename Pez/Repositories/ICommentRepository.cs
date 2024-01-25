using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface ICommentRepository:IBaseRepository<Comments, Guid>
    {
        Task<Comments> GetWithParentAsync(Guid id);
        Task<List<Comments>> GetAllAsync();
        Task<List<Comments>> GetProductCommentsAsync();
        Task<List<Comments>> GetBlogCommentsAsync();
    }
}
