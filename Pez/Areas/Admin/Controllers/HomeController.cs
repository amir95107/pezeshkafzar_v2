using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public HomeController(
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LastProductsList()
        {
            var products = await _productRepository.GetLastAddedProductsAsync(10);
            return PartialView(products);
        }

        public async Task<IActionResult> Order()
        {
            return PartialView(await _orderRepository.GetOrdersAsync());
        }

        public IActionResult Media()
        {
            //var images = db.Products.Select(p=>p.ImageName).ToList();
            //ViewBag.Images = images;
            return View();
        }
        //public IActionResult MediaPartial()
        //{
        //    var images = db.Products.Select(p => p.ImageName).Distinct().ToList();
        //    ViewBag.Images = images;
        //    return PartialView();
        //}
    }
}