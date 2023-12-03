using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;
using Pezeshkafzar_v2.ViewModels;
using System.Threading;

namespace Pezeshkafzar_v2.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private Guid CurrentUserId;

        public ProductController(
            IHttpContextAccessor accessor,
            IProductRepository productRepository,
            IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            CurrentUserId = accessor.HttpContext.User.Identity.IsAuthenticated ? Guid.Parse(accessor.HttpContext.User.Claims.FirstOrDefault().Value) : Guid.Empty;
        }




        public async Task<IActionResult> LastProduct()
            => PartialView(await _productRepository.GetLastAddedProductsAsync(3));

        public async Task<IActionResult> SpecialOffer()
            => PartialView(await _productRepository.GetSpecialProductsAsync());

        [Route("AllOffers")]
        public async Task<IActionResult> AllOffers(int take = 16, int skip = 0)
        {
            var products = await _productRepository.GetAllProductsWithDiscountAsync(take, skip);

            var pageCount = products.Count() / take + 1;
            if (products.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = (skip / take) + 1;
            return View(products);
        }

        public async Task<IActionResult> LastSeenProducts()
        {
            var cookies = Request.Cookies
                .Where(l => l.Key.StartsWith("SeenProduct-") && !string.IsNullOrWhiteSpace(l.Value));
            List<Products> lastSeen = new List<Products>();
            int seen = 0;
            foreach (var item in cookies)
            {
                if (seen >= 5)
                    break;
                var product = await _productRepository.FindAsync(Guid.Parse(item.Value));
                if (product != null)
                {
                    lastSeen.Add(product);
                }
                seen++;
            }

            return PartialView(lastSeen);
        }




        [Route("product/{id}/{sefUrl}")]
        public async Task<IActionResult> ShowProduct(Guid id, string sefUrl)
        {
            var product = await _productRepository.ProductDetailAsync(id);
            if (product != null)
            {
                if (product.IsAcceptedByAdmin)
                {
                    if (product.SefUrl != sefUrl)
                    {
                        return RedirectPermanent($"/ShowProduct/{id}/{product.SefUrl}");
                    }
                    ViewBag.LastUpdate = TimeHelper.Detail((DateTime)product.LastUpdated);
                    ViewBag.Galleries = await _productRepository.GetProductGalleriesAsync(product.Id);
                    //ViewBag.ProductFeatures = product.Product_Features.DistinctBy(f=>f.FeatureID).Select(f=>new ShowProductFeatureViewModel()
                    //{
                    //    FeatureTitle = f.Features.FeatureTitle,
                    //    Values = db.Product_Features.Where(fe=>fe.FeatureID==f.FeatureID).Select(fe=>fe.Value).ToList()
                    //}).ToList();
                    ViewBag.Size = await _productRepository.GetProductFeaturesAsync(product.Id, Guid.Parse("2d9436a0-0dc2-4a8a-a009-e36d0275b6f9"));
                    if (product == null)
                    {
                        return NotFound();
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        ViewBag.IsLiked = await _productRepository.IsProductLiked(CurrentUserId, product.Id);
                    }
                    SetCookie($"SeenProduct-{product.UniqueKey}", product.Id.ToString(), 7);
                    //product.Visit++;

                    return View(product);
                }
                else
                {
                    return Redirect($"/PageNotFound?aspxerrorpath={Request.Path}");
                }
            }
            else
            {
                return Redirect($"/PageNotFound?aspxerrorpath={Request.Path}");
            }
        }

        public async Task<IActionResult> ShowComments(Guid id)
        {
            return PartialView(await _productRepository.GetProductCommentsAsync(id));
        }

        [Route("Product/CreateComment/{id}")]
        public ActionResult CreateComment(Guid id)
        {
            return PartialView(new Comments()
            {
                ProductID = id
            });
        }

        [Authorize]
        [HttpPost]
        [Route("Product/CreateComment/{id}")]
        public async Task<IActionResult> CreateComment(Comments productComment, Guid id)
        {
            if (ModelState.IsValid)
            {
                productComment.ProductID = id;
                productComment.CreateDate = DateTime.Now;
                productComment.IsShow = false;

                var product = await _productRepository.FindAsync(id);
                product.Comments.Add(productComment);
                _productRepository.Modify(product);
                await _productRepository.SaveChangesAsync();
                SendEmail.Send("a.janmohammadi@gmail.com", "",
                    "کامنت برای محصول " + productComment.Products.Title,
                    "یک نظر برای محصول " + productComment.Products.Title + " از طرف " + productComment.Name + " اومده.");

                return PartialView("ShowComments",
                    await _productRepository.GetProductCommentsAsync(id));
            }
            return PartialView(productComment);
        }

        [Route("Archive")]
        public async Task<IActionResult> ArchiveProduct(int pageId = 1, string title = "", decimal minPrice = 0, decimal maxPrice = 0, List<Guid>? selectedGroups = null, Guid? brandId = null)
        {
            ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
            ViewBag.productTitle = title;

            ViewBag.pageId = pageId;
            ViewBag.NoProducts = "محصولی با مشخصات وارد شده موجود نیست!";
            ViewBag.selectGroup = selectedGroups;
            List<Products> list = new List<Products>();
            int take = 12;
            int skip = (pageId - 1) * take;
            list.AddRange(await _productRepository.GetProductListAsync(take, skip, title, minPrice, maxPrice, selectedGroups, brandId));

            if (list.Any())
            {
                ViewBag.minPrice = list.Select(p => p.Price).Min();
                ViewBag.maxPrice = list.Select(p => p.Price).Max();
            }
            else
            {
                var productsMinMaxPrice = await _productRepository.GetMinMaxPriceOfAllProducts();
                ViewBag.minPrice = productsMinMaxPrice["minPrice"];
                ViewBag.maxPrice = productsMinMaxPrice["maxPrice"];
            }

            list = list.Distinct().ToList();
            ViewBag.Take = take;
            if (list != null)
            {
                ViewBag.ProductsCount = list.Count();
                ViewBag.PageCount = list.Count() / take + 1;
            }
            return View(list.OrderByDescending(p => p.CreateDate).Skip(skip).Take(take).ToList());
        }

        public async Task<IActionResult> RelatedProducts(Guid id)
        {
            return PartialView(await _productRepository.GetRelatedProductsAsync(id, 6));
        }


        public async Task<IActionResult> ShowBrand()
        {
            return PartialView(await _brandRepository.GetAllBrandsAsync());
        }

        [Route("ShowAllBrands")]
        public async Task<IActionResult> ShowAllBrands()
        {
            return View(await _brandRepository.GetAllBrandsAsync());
        }

        [Route("Product/QuickView/{id}")]
        public async Task<IActionResult> QuickView(Guid id)
        {
            return PartialView(await _productRepository.FindAsync(id));
        }

        //public ActionResult SpecialProducts()
        //{
        //    return PartialView(db.SpecialProducts.Where(sp => sp.ExpireDate > DateTime.Now && sp.CreateDate < DateTime.Now && sp.IsActive));
        //}

        public async Task<IActionResult> BestSellings()
        {
            return PartialView(await _productRepository.GetBestSellingsProductsAsync());
        }

        public async Task<IActionResult> BestSellingsInBlog()
        {
            var products = (await _productRepository.GetBestSellingsProductsAsync())
                .Select(p => new BestSellingsInBlog { ProductID = p.Id, Title = p.Title, SefUrl = p.SefUrl, Image = p.ImageName, Price = p.Price, PriceAfterDiscount = p.PriceAfterDiscount, Date = p.CreateDate })
                .OrderByDescending(p => p.Date).Take(5);
            return PartialView(products);
        }

        [Route("Categories")]
        public ActionResult Categories()
        {
            return View();
        }

        public async Task<IActionResult> CategoriesPartial(Guid? parentId, Guid? groupId)
        {
            var cats = await _productRepository.GetProductGroupsAsync(true);
            var products = await _productRepository.GetProductSelectedGroupsAsync(parentId.Value);
            //var products = db.Product_Selected_Groups.Where(p => p.GroupID == parentId).Select(p => p.Products).Where(p => p.IsActive && p.IsAcceptedByAdmin).OrderByDescending(p => p.CreateDate).Take(12).ToList();
            if (groupId != null && parentId == null)
            {
                cats = await _productRepository.GetProductGroupsAsync(null, groupId.Value);
            }
            else if (groupId == null && parentId != null)
            {
                cats = await _productRepository.GetProductGroupsAsync(parentId.Value, null);
            }
            else if (groupId != null && parentId != null)
            {
                cats = await _productRepository.GetProductGroupsAsync(parentId.Value, groupId.Value);
            }
            ViewBag.Products = products;
            ViewBag.ParentId = parentId;
            ViewBag.GroupId = groupId;
            return PartialView(cats);
        }
    }
}