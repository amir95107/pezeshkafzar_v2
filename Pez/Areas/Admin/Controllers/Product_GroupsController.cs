using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class Product_GroupsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupRepository;

        public Product_GroupsController(
            IProductRepository productRepository,
            IProductGroupRepository productGroupRepository)
        {
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
        }

        // GET: Admin/Product_Groups
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListGroups()
        {
            var product_Groups = await _productRepository.GetProductGroupsAsync(true);
            return PartialView(product_Groups.ToList());
        }

        // GET: Admin/Product_Groups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product_Groups = await _productRepository.GetGroupAsync(id.Value);
            if (product_Groups == null)
            {
                return NotFound();
            }
            return View(product_Groups);
        }

        // GET: Admin/Product_Groups/Create
        [HttpPost]
        public async Task<IActionResult> PreCreate(int? key)
        {
            var parentId = key != null ? await _productRepository.GetGroupParentIdByKeyAsync(key.Value) : null;
            return PartialView(new Product_Groups()
            {
                ParentID= parentId,
                UniqueKey = key.GetValueOrDefault()
            });
        }

        // POST: Admin/Product_Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product_Groups product_Groups)
        {
            ViewBag.ParentID = new SelectList(await _productRepository.GetProductGroupsAsync(false), "GroupID", "GroupTitle", product_Groups.ParentID);
            if (true)
            {
                var uniqueKey = await _productRepository.GetLastGroupNumberAsync();
                product_Groups.UniqueKey = uniqueKey + 1;
                await _productRepository.AddGroupAsync(product_Groups);
                await _productRepository.SaveChangesAsync();
                return PartialView("ListGroups", await _productRepository.GetProductGroupsAsync(true));
            }
            else
            {
                return View(product_Groups);
            }



        }

        //GET: Admin/Product_Groups/Edit/5
        public async Task<IActionResult> Edit(int key)
        {
            Product_Groups product_Groups = await _productGroupRepository.FindByKeyAsync(key);
            if (product_Groups == null)
            {
                return StatusCode(422);
            }

            return PartialView(product_Groups);
        }

        // POST: Admin/Product_Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product_Groups product_Groups)
        {
            _productGroupRepository.Modify(product_Groups);
            await _productGroupRepository.SaveChangesAsync();
            return PartialView("ListGroups", await _productRepository.GetProductGroupsAsync(true));
        }

        // GET: Admin/Product_Groups/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var group = await _productGroupRepository.FindAsync(id);
            if (group.Children.Any())
            {
                foreach(var child in group.Children)
                {
                    child.RemovedAt = DateTime.Now;
                }
            }
            _productGroupRepository.Modify(group);
            await _productGroupRepository.SaveChangesAsync();
            return PartialView("ListGroups", await _productRepository.GetProductGroupsAsync(true));
        }

        //// POST: Admin/Product_Groups/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    Product_Groups product_Groups = db.Product_Groups.Find(id);
        //    db.Product_Groups.Remove(product_Groups);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //public Task<IActionResult> VisitCountCategories()
        //{
        //    return View(db.Product_Groups.Where(p => p.Product_Selected_Groups.Any()).ToList());
        //}

        //public Task<IActionResult> ShowMostViewedProduct(int id)
        //{
        //    var product = db.Product_Selected_Groups.Where(g => g.GroupID == id).Select(p => p.Products).OrderByDescending(p=>p.Visit).First();
        //    var od = db.OrderDetails.Where(o => o.ProductID == product.ProductID);
        //    int count = 0;
        //    if (od.Count() > 0)
        //    {
        //        count = od.Sum(p=>p.Count);
        //    }
        //    ViewBag.Count = count;
        //    return PartialView(product);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
