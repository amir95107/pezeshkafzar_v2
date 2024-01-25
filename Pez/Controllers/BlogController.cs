using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Controllers
{
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        // GET: Blog
        [Route("blog/all")]
        public async Task<IActionResult> AllBlogs([FromQuery] string q,int pageId = 1)
        {
            int take = 6;
            int skip = (pageId - 1) * take;
            var blogs = await _blogRepository.GetBlogsAsync(take,skip,q);
            
            var pageCount = blogs.Count() / take + 1;
            if (blogs.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageId;

            return View(blogs);
        }

        public async Task<IActionResult> LastBlogs()
        {
            var lb = await _blogRepository.GetBlogsAsync(3,0,string.Empty);
            return PartialView(lb);
        }

        [Route("blog/{id}/{SefUrl}")]
        public async Task<IActionResult> ShowBlog(Guid id, string SefUrl)
        {
            var blog = await _blogRepository.FindAsync(id);
            if (blog != null)
            {
                blog.Visit++;

                try
                {
                    _blogRepository.Modify(blog);
                }
                catch { }

                ViewBag.Title = blog.Title;
                ViewBag.Description = blog.ShortDescription;
                ViewBag.Top = await _blogRepository.GetBlogsAsync(3, 0, string.Empty);
                return View(blog);
            }
            else
            {
                return Redirect("/Error/Not-Found?aspxerrorpath=Blog/" + id + "/" + SefUrl);
                //return HttpNotFound();
            }
        }
    }
}