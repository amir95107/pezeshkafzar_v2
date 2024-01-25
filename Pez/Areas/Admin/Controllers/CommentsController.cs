using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBlogRepository _blogRepository;

        public CommentsController(
            ICommentRepository commentRepository,
            IProductRepository productRepository,
            IBlogRepository blogRepository)
        {
            _commentRepository = commentRepository;
            _productRepository = productRepository;
            _blogRepository = blogRepository;
        }


        // GET: Admin/Comments
        public async Task<IActionResult> Index()
        {
            var comments = await _commentRepository.GetAllAsync();
            return View(comments.ToList());
        }

        // GET: Admin/Comments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var comments = await _commentRepository.FindAsync(id.Value);
            if (comments == null)
            {
                return NotFound();
            }
            return View(comments);
        }

        // GET: Admin/Comments/Create
        //public async Task<IActionResult> Create()
        //{
        //    ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title");
        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title");
        //    return View();
        //}

        public async Task<IActionResult> Create(Guid? id)
        {
            ViewBag.BlogID = new SelectList(await _blogRepository.GetBlogsAsync(50,0,null), "Id", "Title");
            ViewBag.ProductID = new SelectList(await _productRepository.GetAllProductsAsync(), "Id", "Title");
            if (id != null)
            {
                ViewBag.ParentID = id;
                var parent = (await _commentRepository.GetWithParentAsync(id.Value));
                ViewBag.ParentComment = parent != null ? parent.Comment : null;
            }
            return View();
        }

        // POST: Admin/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comments comments)
        {
            if (true)
            {
                comments.CreateDate = DateTime.Now;
                comments.Name = "ادمین";
                await _commentRepository.AddAsync(new Comments
                {
                    Comment=comments.Comment,
                    IsShow = comments.IsShow,
                    Name = comments.Name,
                    ParentID = comments.ParentID,
                    ProductID = comments.ProductID
                });
                await _commentRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Comments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Comments comments = await _commentRepository.FindAsync(id.Value);
            if (comments == null)
            {
                return NotFound();
            }
            var products = await _productRepository.GetAllProductsAsync();
            ViewBag.BlogID = new SelectList(await _blogRepository.GetBlogsAsync(50, 0, null), "BlogID", "Title", comments.BlogID);
            ViewBag.ProductID = new SelectList(products, "ProductID", "Title", comments.ProductID);
            ViewBag.CommentID = new SelectList(products, "CommentID", "Title", comments.Id);
            return View(comments);
        }

        // POST: Admin/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Comments comments)
        {
            if (ModelState.IsValid)
            {
                _commentRepository.Modify(comments);
                await _commentRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BlogID = new SelectList(await _blogRepository.GetBlogsAsync(50, 0, null), "BlogID", "Title", comments.BlogID);
            ViewBag.ProductID = new SelectList(await _productRepository.GetAllProductsAsync(), "ProductID", "Title", comments.ProductID);
            return View(comments);
        }

        // GET: Admin/Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Comments comments = await _commentRepository.FindAsync(id.Value);
            if (comments == null)
            {
                return NotFound();
            }
            return View(comments);
        }

        // POST: Admin/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Comments comments = await _commentRepository.FindAsync(id);
            _commentRepository.Remove(comments);
            await _commentRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ShowInSite(Guid id,bool isShow)
        {
            Comments comments = await _commentRepository.FindAsync(id);
            comments.IsShow = isShow;
            _commentRepository.Modify(comments);
            await _commentRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
