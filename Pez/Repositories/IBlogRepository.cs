using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IBlogRepository : IBaseRepository<Blogs,Guid>
    {
        Task<List<Blogs>> GetBlogsAsync(int take, int skip, string q);
        Task<int> BlogsCountAsync(string q);
        Task<Blogs> GetBlogDetailAsync(Guid id);
        
    }
}