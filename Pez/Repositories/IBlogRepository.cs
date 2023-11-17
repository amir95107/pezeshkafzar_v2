using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IBlogRepository : IBaseRepository<Blogs,Guid>
    {
        Task<List<Blogs>> GetBlogsAsync(int take, int skip, string q,CancellationToken cancellationToken);
        Task<int> BlogsCountAsync(string q,CancellationToken cancellationToken);
        Task<Blogs> GetBlogDetailAsync(Guid id, CancellationToken cancellationToken);
        
    }
}