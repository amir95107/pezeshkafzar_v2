using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class BlogRepository : BaseRepository<Blogs, Guid>, IBlogRepository
    {
        private readonly IQueryable<Blogs> NotRemoved;
        public BlogRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
        }

        public async Task<int> BlogsCountAsync(string q)
        => await NotRemoved.CountAsync(x => !string.IsNullOrWhiteSpace(q) ? x.Title.Contains(q) || x.ShortDescription.Contains(q) || x.Text.Contains(q) : true);

        public async Task<Blogs> GetBlogDetailAsync(Guid id)
        => await NotRemoved.FirstOrDefaultAsync(x=>x.Id == id);

        public async Task<List<Blogs>> GetBlogsAsync(int take, int skip, string q)
        => await NotRemoved.Where(x => !string.IsNullOrWhiteSpace(q) ? x.Title.Contains(q) || x.ShortDescription.Contains(q) || x.Text.Contains(q) : true)
            .Take(take)
            .Skip(skip)
            .ToListAsync(); 
    }
}
