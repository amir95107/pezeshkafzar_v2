using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class PagesController : Controller
    {
        private readonly IPageRepository _pageRepository;

        public PagesController(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        // GET: Admin/Pages
        public async Task<IActionResult> Index()
        {
            return View(await _pageRepository.GetAllAsync());
        }

        // GET: Admin/Pages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Page pages = await _pageRepository.FindAsync(id.Value);
            if (pages == null)
            {
                return NotFound();
            }
            return View(pages);
        }

        // GET: Admin/Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page pages)
        {
            if (ModelState.IsValid)
            {
                await _pageRepository.AddAsync(pages);
                await _pageRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pages);
        }

        // GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Page pages = await _pageRepository.FindAsync(id.Value);
            if (pages == null)
            {
                return NotFound();
            }
            return View(pages);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page pages)
        {
            if (ModelState.IsValid)
            {
                _pageRepository.Modify(pages);
                await _pageRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pages);
        }

        // GET: Admin/Pages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Page pages = await _pageRepository.FindAsync(id.Value);
            if (pages == null)
            {
                return NotFound();
            }
            return View(pages);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Page pages = await _pageRepository.FindAsync(id);
            _pageRepository.Remove(pages);
            await _pageRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
