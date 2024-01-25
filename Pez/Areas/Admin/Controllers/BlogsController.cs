using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;
using System.Net;
using static Pezeshkafzar_v2.Utilities.ImageExtentions;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class BlogsController : Controller
    {
        // GET: Admin/Blogs

        private readonly IBlogRepository _blogRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public BlogsController(
            IBlogRepository blogRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            _blogRepository = blogRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int take = 10, int skip = 0, string? q = null)
        {
            return View(await _blogRepository.GetBlogsAsync(take, skip, q));
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blogs blog, IFormFile imageName)
        {
            if (true)
            {
                blog.Visit = 0;
                blog.CreateDate = DateTime.Now;
                blog.Author = User.Identity.Name;

                if (imageName != null && imageName.IsImage())
                {

                    blog.ImageName = blog.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageName.FileName);
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "BlogImages");
                    imageName.UploadAsync(Path.Combine(imagesPath, blog.ImageName));
                    ImageResizer img300 = new ImageResizer(300);
                    try
                    {
                        img300.Resize(Path.Combine(imagesPath, blog.ImageName), Path.Combine(imagesPath, "thumb", blog.ImageName));
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                if (blog.SefUrl == null || blog.SefUrl == "")
                {
                    blog.SefUrl = blog.Title.Replace(" ", "-");
                }
                await _blogRepository.AddAsync(blog);
                await _blogRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Blogs blog = await _blogRepository.FindAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Blogs blog, IFormFile imageName)
        {
            if (true)
            {
                if (imageName != null)
                {
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "Blogs");

                    if (blog.ImageName != null)
                    {

                        System.IO.File.Delete(Path.Combine(imagesPath, blog.ImageName));
                        System.IO.File.Delete(Path.Combine(imagesPath,"thumb",blog.ImageName));
                    }
                    blog.ImageName = imageName.FileName.Split('.')[0];

                    blog.ImageName = blog.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageName.FileName);
                    blog.ImageName = blog.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageName.FileName);
                    imageName.UploadAsync(Path.Combine(imagesPath, blog.ImageName));
                    ImageResizer img300 = new ImageResizer(300);
                    try
                    {
                        img300.Resize(Path.Combine(imagesPath, blog.ImageName), Path.Combine(imagesPath, "thumb", blog.ImageName));
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                if (blog.SefUrl == null || blog.SefUrl == "")
                {
                    blog.SefUrl = blog.Title.Replace(" ", "-");
                }

                _blogRepository.Modify(blog);
                await _blogRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            Blogs blog = await _blogRepository.FindAsync(id);
            _blogRepository.Remove(blog);
            await _blogRepository.SaveChangesAsync();
            return View("index", await _blogRepository.GetBlogsAsync(50,0,null));
        }

    }
}