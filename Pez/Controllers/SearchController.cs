using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Controllers
{
    public class SearchController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public SearchController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string q, int pageId = 1)
        {
            if (q == null)
            {
                return RedirectToAction("MobileSearch");
            }
            List<Products> list = new List<Products>();

            var queries = System.Web.HttpUtility.ParseQueryString(string.Empty);
            //if (queries["ref"] == "tag")
            //{
            //    list.AddRange(await _productRepository.GetProductsByTagsAsync(q));
            //}
            //List<Products> products = db.Products.Where(p => p.IsActive && p.IsAcceptedByAdmin).ToList();
            list.AddRange(await _productRepository.GetProductsByTagsAsync(q));
            var searched = q.Split(' ').Where(qq => qq.Length >= 3);
            if (!q.Split(' ').Any(s => s.Length >= 3))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(await _productRepository.GetProductListAsync(12, pageId-1, q,0,0,null,null));
            }
            
            ViewBag.search = q;
            int take = 12;
            int skip = (pageId - 1) * take;
            ViewBag.pageId = pageId;
            ViewBag.Take = take;
            if (list != null)
            {
                ViewBag.ProductsCount = list.Count();
                ViewBag.PageCount = list.Count() / take + 1;
            }
            return View(list.Distinct().Skip(skip).Take(take).ToList());
        }

        public async Task<IActionResult> SearchSuggestion(string q)
        {
            List<Products> list = new List<Products>();
            var searched = q.Split(' ').Where(qq => qq.Length >= 4);
            if (!q.Split(' ').Any(s => s.Length >= 4))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(await _productRepository.GetProductListAsync(12, 0, q, 0, 0, null, null));
            }
            if (q == null || q == "" || q == " ")
            {
                list.Clear();
            }
            ViewBag.search = q;
            list = list.Distinct().ToList();
            ViewBag.Count = list.Count;
            return PartialView(list);
        }

        //private int ProductBrand(Guid id)
        //{
        //    var product = db.Products.Find(id);
        //    var brandId = product.ProductBrand.Single(pb => pb.ProductID == product.ProductID).BrandID;
        //    return brandId;
        //}

        public IActionResult MobileSearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MobileSearchSuggestion(string q)
        {
            List<Products> list = new List<Products>();
            var searched = q.Split(' ').Where(qq => qq.Length >= 4);
            if (!q.Split(' ').Any(s => s.Length >= 4))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(await _productRepository.GetProductListAsync(12, 0, q, 0, 0, null, null));
            }

            if (q == null || q == "" || q == " ")
            {
                list.Clear();
            }
            ViewBag.search = q;
            ViewBag.Count = list.Count;
            return PartialView(list.Distinct().OrderByDescending(p=>p.ShowOrder));
        }

        public async Task<IActionResult> LastSearchedProducts()
        {
            var cookieList = Request.Cookies;
            
            List<Products> lastSearched = new List<Products>();
            foreach (var item in cookieList.Where(l => l.Key.StartsWith("SearchedProduct-")))
            {
                var product = await _productRepository.FindAsync(Guid.Parse(item.Value));
                if (product != null)
                {
                    lastSearched.Add(product);
                }
            }

            return PartialView(lastSearched.Take(4));
        }

        public void SetLastSearchedCookie(Guid id)
        {
            SetCookie("SearchedProduct-" + id.ToString(), id.ToString(), 30);
        }
    }
}