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

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        


        public async Task<IActionResult> LastProduct(CancellationToken cancellationToken)
            => PartialView(await _productRepository.GetLastAddedProductsAsync(3, cancellationToken));

        public async Task<IActionResult> SpecialOffer(CancellationToken cancellationToken)
            => PartialView(await _productRepository.GetSpecialProductsAsync(cancellationToken));

        [Route("AllOffers")]
        public async Task<IActionResult> AllOffers(CancellationToken cancellationToken, int take = 16, int skip = 0)
        {
            var products = await _productRepository.GetAllProductsWithDiscountAsync(take, skip, cancellationToken);

            var pageCount = products.Count() / take + 1;
            if (products.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = (skip / take) + 1;
            return View(products);
        }

        public async Task<IActionResult> LastSeenProducts(CancellationToken cancellationToken)
        {
            var cookies = Request.Cookies
                .Where(l => l.Key.StartsWith("SeenProduct-") && !string.IsNullOrWhiteSpace(l.Value));
            List<Products> lastSeen = new List<Products>();
            int seen = 0;
            foreach (var item in cookies)
            {
                if (seen >= 5)
                    break;
                var product = await _productRepository.FindAsync(Guid.Parse(item.Value), cancellationToken);
                if (product != null)
                {
                    lastSeen.Add(product);
                }
                seen++;
            }

            return PartialView(lastSeen);
        }




        [Route("ShowProduct/{id}/{sefUrl}")]
        public async Task<IActionResult> ShowProduct(string uniqueKey, string sefUrl, CancellationToken cancellationToken)
        {
            var product = await _productRepository.ProductDetailAsync(uniqueKey, cancellationToken);
            if (product != null)
            {
                if (product.IsAcceptedByAdmin)
                {
                    if (product.SefUrl != sefUrl)
                    {
                        return RedirectPermanent($"/ShowProduct/{uniqueKey}/{product.SefUrl}");
                    }
                    ViewBag.LastUpdate = TimeHelper.Detail((DateTime)product.LastUpdated);
                    ViewBag.Galleries = await _productRepository.GetProductGalleriesAsync(product.Id, cancellationToken);
                    //ViewBag.ProductFeatures = product.Product_Features.DistinctBy(f=>f.FeatureID).Select(f=>new ShowProductFeatureViewModel()
                    //{
                    //    FeatureTitle = f.Features.FeatureTitle,
                    //    Values = db.Product_Features.Where(fe=>fe.FeatureID==f.FeatureID).Select(fe=>fe.Value).ToList()
                    //}).ToList();
                    ViewBag.Size = await _productRepository.GetProductFeaturesAsync(product.Id, Guid.Parse(""), cancellationToken);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        ViewBag.IsLiked = await _productRepository.IsProductLiked(Guid.Empty, product.Id, cancellationToken);
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

        public async Task<IActionResult> ShowComments(Guid id, CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.GetProductCommentsAsync(id, cancellationToken));
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
        public async Task<IActionResult> CreateComment(Comments productComment, Guid id, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                productComment.ProductID = id;
                productComment.CreateDate = DateTime.Now;
                productComment.IsShow = false;

                var product = await _productRepository.FindAsync(id, cancellationToken);
                product.Comments.Add(productComment);
                _productRepository.Modify(product);
                await _productRepository.SaveChangesAsync(cancellationToken);
                SendEmail.Send("a.janmohammadi@gmail.com", "",
                    "کامنت برای محصول " + productComment.Products.Title,
                    "یک نظر برای محصول " + productComment.Products.Title + " از طرف " + productComment.Name + " اومده.");

                return PartialView("ShowComments",
                    await _productRepository.GetProductCommentsAsync(id, cancellationToken));
            }
            return PartialView(productComment);
        }

        [Route("Archive")]
        public async Task<IActionResult> ArchiveProduct(CancellationToken cancellationToken, int pageId = 1, string title = "", decimal minPrice = 0, decimal maxPrice = 0, List<Guid>? selectedGroups = null, Guid? brandId = null)
        {
            ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false, cancellationToken);
            ViewBag.productTitle = title;

            ViewBag.pageId = pageId;
            ViewBag.NoProducts = "محصولی با مشخصات وارد شده موجود نیست!";
            ViewBag.selectGroup = selectedGroups;
            List<Products> list = new List<Products>();
            int take = 12;
            int skip = (pageId - 1) * take;
            list.AddRange(await _productRepository.GetProductListAsync(take, skip, title, minPrice, maxPrice, selectedGroups, brandId, cancellationToken));

            if (list.Any())
            {
                ViewBag.minPrice = list.Select(p => p.Price).Min();
                ViewBag.maxPrice = list.Select(p => p.Price).Max();
            }
            else
            {
                var productsMinMaxPrice = await _productRepository.GetMinMaxPriceOfAllProducts(cancellationToken);
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

        public async Task<IActionResult> RelatedProducts(Guid id, CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.GetRelatedProductsAsync(id, 6, cancellationToken));
        }


        public async Task<IActionResult> ShowBrand(CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.GetAllBrandsAsync(cancellationToken));
        }

        [Route("ShowAllBrands")]
        public async Task<IActionResult> ShowAllBrands(CancellationToken cancellationToken)
        {
            return View(await _productRepository.GetAllBrandsAsync(cancellationToken));
        }

        [Route("Product/QuickView/{id}")]
        public async Task<IActionResult> QuickView(Guid id, CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.FindAsync(id, cancellationToken));
        }

        //public ActionResult SpecialProducts()
        //{
        //    return PartialView(db.SpecialProducts.Where(sp => sp.ExpireDate > DateTime.Now && sp.CreateDate < DateTime.Now && sp.IsActive));
        //}

        public async Task<IActionResult> BestSellings(CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.GetBestSellingsProductsAsync(cancellationToken));
        }

        public async Task<IActionResult> BestSellingsInBlog(CancellationToken cancellationToken)
        {
            var products = (await _productRepository.GetBestSellingsProductsAsync(cancellationToken))
                .Select(p => new BestSellingsInBlog { ProductID = p.Id, Title = p.Title, SefUrl = p.SefUrl, Image = p.ImageName, Price = p.Price, PriceAfterDiscount = p.PriceAfterDiscount, Date = p.CreateDate })
                .OrderByDescending(p => p.Date).Take(5);
            return PartialView(products);
        }

        [Route("Categories")]
        public ActionResult Categories()
        {
            return View();
        }

        public async Task<IActionResult> CategoriesPartial(Guid? parentId, Guid? groupId, CancellationToken cancellationToken)
        {
            var cats = await _productRepository.GetProductGroupsAsync(true, cancellationToken);
            var products = await _productRepository.GetProductSelectedGroupsAsync(parentId.Value, cancellationToken);
            //var products = db.Product_Selected_Groups.Where(p => p.GroupID == parentId).Select(p => p.Products).Where(p => p.IsActive && p.IsAcceptedByAdmin).OrderByDescending(p => p.CreateDate).Take(12).ToList();
            if (groupId != null && parentId == null)
            {
                cats = await _productRepository.GetProductGroupsAsync(null, groupId.Value, cancellationToken);
            }
            else if (groupId == null && parentId != null)
            {
                cats = await _productRepository.GetProductGroupsAsync(parentId.Value, null, cancellationToken);
            }
            else if (groupId != null && parentId != null)
            {
                cats = await _productRepository.GetProductGroupsAsync(parentId.Value, groupId.Value, cancellationToken);
            }
            ViewBag.Products = products;
            ViewBag.ParentId = parentId;
            ViewBag.GroupId = groupId;
            return PartialView(cats);
        }
    }
}