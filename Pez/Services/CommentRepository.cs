using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class CommentRepository : BaseRepository<Comments, Guid>, ICommentRepository
    {
        public CommentRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }

        public async Task<List<Comments>> GetAllAsync()
            => await Entities
            .Include(x=>x.Products)
            .Include(x=>x.Blogs)
            .ToListAsync();



        public async Task<List<Comments>> GetBlogCommentsAsync()
            => await Entities.Include(x=>x.Blogs).ToListAsync();

        public async Task<List<Comments>> GetProductCommentsAsync()
            => await Entities.Include(x=>x.Products).ToListAsync();

        public async Task<Comments> GetWithParentAsync(Guid id)
            => await Entities
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x=>x.Id == id);
    }
}
