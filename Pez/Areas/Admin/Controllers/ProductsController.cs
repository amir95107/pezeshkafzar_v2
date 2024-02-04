using DataLayer.Models;
using DataLayer.Models.Events;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.SqlServer.Server;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;
using Pezeshkafzar_v2.ViewModels;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using static Pezeshkafzar_v2.Utilities.ImageExtentions;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    //[Route("products")]
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IBrandRepository _brandRepository;
        private readonly IFeatureRepository _featureRepository;

        public ProductsController(
            IWebHostEnvironment hostingEnvironment,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            IFeatureRepository featureRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _featureRepository = featureRepository;
        }

        #region Products
        // GET: Admin/Products

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductsList([FromQuery] int page = 1, [FromQuery] int take = 20)
        {
            var products = await _productRepository.GetAllProductsAsync(take, take * (page - 1));
            ViewBag.PageCount = (products.Count) / take + 1;
            ViewBag.PageNumber = page;
            ViewBag.Take = take;
            return PartialView(products);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var products = await _productRepository.FindAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection input, List<Guid> selectedGroups, IFormFile imageProduct, string tags)
        {
            var products = new Products
            {
                UniqueKey = StringExtensions.RandomString(6),
                Garanty = input["Garanty"],
                ImageName = input["ImageName"],
                IsInBestselling = bool.Parse(input["IsInBestselling"].ToString().Split(',')[0]),
                Price = decimal.Parse(input["Price"]),
                PriceAfterDiscount = decimal.Parse(input["PriceAfterDiscount"]),
                SefUrl = input["SefUrl"],
                ShortDescription = input["ShortDescription"],
                Stock = int.Parse(input["Stock"]),
                Text = input["Text"],
                Title = input["Title"],
                CreateDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                LikeCount = 0,
                Point = 0,
                IsAcceptedByAdmin = true,
                IsActive = true,
                Visit = 0
            };
            if (ModelState.IsValid)
            {
                if (selectedGroups == null)
                {
                    ViewBag.ErrorSelectedGroup = true;
                    ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
                    return View(products);
                }
                products.ImageName = imageProduct.FileName.Split('.')[0];
                if (imageProduct != null && imageProduct.IsImage())
                {

                    products.ImageName = products.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "ProductImages");
                    var src = Path.Combine(imagesPath, products.ImageName);
                    await imageProduct.UploadAsync(src);
                    ImageResizer img300 = new ImageResizer(300);
                    try
                    {
                        img300.Resize(src, Path.Combine(imagesPath, "thumb", products.ImageName));
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
                products.IsInBestselling = false;
                if (products.SefUrl == null || products.SefUrl == "")
                {
                    products.SefUrl = products.Title.Replace(" ", "-");
                }
                else
                {
                    products.SefUrl = products.SefUrl.Replace(" ", "-");
                }


                //foreach (int selectedGroup in selectedGroups)
                //{
                //    db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                //    {
                //        ProductID = products.ProductID,
                //        GroupID = selectedGroup
                //    });
                //}

                var psg = new List<Product_Selected_Groups>();
                foreach (var id in selectedGroups)
                {
                    psg.Add(new()
                    {
                        GroupID = id,
                        ProductID = products.Id
                    });
                }
                products.Product_Selected_Groups = psg;

                var productTags = new List<Product_Tags>();
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tagList = tags.Split('#');
                    foreach (string tag in tagList)
                    {
                        productTags.Add(new()
                        {
                            ProductID = products.Id,
                            Tag = tag.Trim()
                        });
                    }

                    products.Product_Tags = productTags;
                }

                await _productRepository.AddAsync(products);
                await _productRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
            return View(new CreateProductViewModel
            {
                UniqueKey = StringExtensions.RandomString(6),
                Garanty = input["Garanty"],
                ImageName = input["ImageName"],
                IsInBestselling = bool.Parse(input["IsInBestselling"].ToString().Split(',')[0]),
                Price = decimal.Parse(input["Price"]),
                PriceAfterDiscount = decimal.Parse(input["PriceAfterDiscount"]),
                SefUrl = input["SefUrl"],
                ShortDescription = input["ShortDescription"],
                Stock = int.Parse(input["Stock"]),
                Text = input["Text"],
                Title = input["Title"],
                CreateDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                LikeCount = 0,
                Point = 0,
                IsAcceptedByAdmin = true,
                IsActive = true,
                Visit = 0
            });
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var products = await _productRepository.FindProductWithChildrenAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            ViewBag.SelectedGroups = products.Product_Selected_Groups.ToList();
            ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
            ViewBag.Tags = string.Join(",", products.Product_Tags.Select(t => t.Tag).ToList());
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection input, List<Guid> selectedGroups, IFormFile imageProduct, string tags)
        {
            var products = await _productRepository.FindProductWithChildrenAsync(Guid.Parse(input["Id"]));


            if (ModelState.IsValid)
            {
                if (selectedGroups == null)
                {
                    ViewBag.ErrorSelectedGroup = true;
                    ViewBag.Groups = await _productRepository.GetProductGroupsAsync(false);
                    return View(products);
                }

                if (products is null)
                    return BadRequest();

                if (imageProduct != null && imageProduct.IsImage())
                {

                    products.ImageName = products.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "ProductImages");
                    var src = Path.Combine(imagesPath, products.ImageName);
                    await imageProduct.UploadAsync(imagesPath);
                    ImageResizer img300 = new ImageResizer(300);
                    try
                    {
                        img300.Resize(src, Path.Combine(imagesPath, "thumb", products.ImageName));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (products.SefUrl == null || products.SefUrl == "")
                {
                    products.SefUrl = products.Title.Replace(" ", "-");
                }
                else
                {
                    products.SefUrl = products.SefUrl.Replace(" ", "-");
                }

                products.LastUpdated = DateTime.Now;

                var inputTags = input["Tags"].ToString().Split(',');
                //if (inputTags != products.Product_Tags.Select(x => x.Tag).ToArray())
                if (CompareArrays(inputTags, products.Product_Tags.Select(x => x.Tag).ToArray()))
                {
                    if (inputTags.Length == 1 && inputTags.All(x => string.IsNullOrWhiteSpace(x)))
                    {
                        products.Product_Tags.Clear();
                    }
                    else
                    {
                        var updatedTags = new List<Product_Tags>();
                        foreach (string tag in inputTags)
                        {
                            updatedTags.Add(new()
                            {
                                ProductID = products.Id,
                                Tag = tag
                            });
                        }

                        products.Product_Tags = updatedTags;
                    }
                }

                if (selectedGroups.Any(x => !products.Product_Selected_Groups.Select(x => x.GroupID).Contains(x)))
                {
                    products.Product_Selected_Groups = selectedGroups.Select(x => new Product_Selected_Groups
                    {
                        ProductID = products.Id,
                        GroupID = x
                    }).ToList();
                }

                //_productRepository.Modify(products);
                await _productRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        private bool CompareArrays(string[] arr1, string[] arr2)
        {
            foreach (var item in arr1)
            {
                if (!arr2.Contains(item))
                    return true;
            }
            return false;
        }

        public async Task<IActionResult> SearchInTags(string tag)
        {

            List<string> tags = (await _productRepository.GetTagsAsync(tag)).Select(x => x.Tag).ToList();
            return Json(tags);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Products products = await _productRepository.FindAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        //// POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Products products = await _productRepository.FindAsync(id);
            products.RemovedAt = DateTime.Now;

            _productRepository.Modify(products);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Gallery

        public async Task<IActionResult> Gallery(Guid id)
        {
            ViewBag.Galleries = await _productRepository.GetProductGalleriesAsync(id);
            Products product = await _productRepository.FindAsync(id);
            ViewBag.PTitle = product.Title;
            ViewBag.Image = product.ImageName;
            return View(new Product_Galleries()
            {
                ProductID = id
            });
        }

        public class GenerateAntiforgeryTokenCookieAttribute : ResultFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext context)
            {
                var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

                // Send the request token as a JavaScript-readable cookie
                var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);

                context.HttpContext.Response.Cookies.Append(
                    "RequestVerificationToken",
                    tokens.RequestToken,
                    new CookieOptions() { HttpOnly = false });
            }

            public override void OnResultExecuted(ResultExecutedContext context)
            {
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
        {
            public void OnResourceExecuting(ResourceExecutingContext context)
            {
                var factories = context.ValueProviderFactories;
                factories.RemoveType<FormValueProviderFactory>();
                factories.RemoveType<FormFileValueProviderFactory>();
                factories.RemoveType<JQueryFormValueProviderFactory>();
            }

            public void OnResourceExecuted(ResourceExecutedContext context)
            {
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Gallery(Product_Galleries gallery, IFormFile imgUp)
        {
            //var gallery = new ProductGalleryViewModel
            //{
            //    ImageName = galleries["ImageName"],
            //    ProductID = Guid.Parse(galleries["ProductID"]),
            //    Title = galleries["Title"]
            //};
            if (!string.IsNullOrWhiteSpace(gallery.Title))
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    Products products = await _productRepository.GetProductWithGalleriesAsync(gallery.ProductID);
                    var type = GalleryType.Image;
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "ProductImages");
                    if (!imgUp.FileName.Contains(".mp4"))
                    {
                        ImageResizer img300 = new ImageResizer(300);
                        try
                        {
                            img300.Resize(Path.Combine(imagesPath, products.ImageName), Path.Combine(imagesPath, "thumb", products.ImageName));
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "vidoes");
                        type = GalleryType.Video;
                    }
                    imagesPath = Path.Combine(imagesPath, products.ImageName);
                    await imgUp.UploadAsync(imagesPath);

                    products.Apply(new ProductGalleryChanged
                    {
                        Title = gallery.Title,
                        ImageName = imagesPath,
                        ProductId = products.Id,
                        GalleryType = type
                    });
                    _productRepository.Modify(products);
                    await _productRepository.SaveChangesAsync();
                }
            }

            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        public async Task<IActionResult> DeleteGallery(Guid id)
        {
            var product = await _productRepository.GetGalleryWithproductAsync(id);
            if (product is null)
                return StatusCode(422);

            var gallery = product.Product_Galleries.FirstOrDefault(x => x.Id == id);

            product.Apply(new ProductGalleryChanged
            {
                EventType = EventType.Delete,
                GalleryId = gallery.Id
            });
            string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "ProductImages");
            System.IO.File.Delete(Path.Combine(imagesPath, gallery.ImageName));
            System.IO.File.Delete(Path.Combine(imagesPath, "thumb", gallery.ImageName));

            _productRepository.Modify(product);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        public async Task<IActionResult> SetAsMainImage(Guid GalleryID, Guid id)
        {
            var product = await _productRepository.GetProductWithGalleriesAsync(id);
            if (product is null)
                return StatusCode(422);

            var gallery = product.Product_Galleries.FirstOrDefault(x => x.Id == GalleryID);
            if (gallery is null)
                return StatusCode(422);

            var productImage = product.ImageName;
            product.ImageName = gallery.ImageName;
            gallery.ImageName = productImage;
            _productRepository.Modify(product);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        #endregion

        #region  Featurs

        public async Task<IActionResult> ProductFeaturs(Guid id)
        {
            ViewBag.Product = await _productRepository.FindAsync(id);
            ViewBag.Features = await _productRepository.GetProductFeaturesAsync(id);
            ViewBag.FeatureID = new SelectList(await _productRepository.GetAllFeaturesAsync(), "Id", "FeatureTitle");
            return View(new Product_Features()
            {
                ProductID = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> ProductFeaturs(Product_Features feature)
        {
            if (true)
            {
                //var product = await _productRepository.GetProductWithFeaturesAsync(feature.ProductID);
                //product.Apply(new ProductFeatureChanged
                //{
                //    FeatureId = feature.FeatureID,
                //    ProductId = feature.ProductID,
                //    Value = feature.Value
                //});
                //_productRepository.Modify(product);

                await _productRepository.AddProductFeature(new Product_Features
                {
                    FeatureID = feature.FeatureID,
                    ProductID = feature.ProductID,
                    Value = feature.Value
                });
                await _productRepository.SaveChangesAsync();
            }

            return RedirectToAction("ProductFeaturs", new { id = feature.ProductID });
        }

        public async Task<IActionResult> DeleteFeature(Guid id)
        {
            await _productRepository.RemoveProductFeatures([id]);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        #endregion

        #region Brands
        public async Task<IActionResult> Brands()
        {
            ViewBag.Brands = await _brandRepository.GetAllBrandsAsync();
            return View(new Brands());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrands(Brands brand, IFormFile BrandImageName)
        {

            if (brand.BrandTitle != null && BrandImageName != null)
            {
                brand.BrandImageName = BrandImageName.FileName;
                if (BrandImageName != null && BrandImageName.IsImage())
                {
                    brand.BrandImageName = BrandImageName.FileName.Split('.')[0] + "_" + Guid.NewGuid().ToString() + Path.GetExtension(BrandImageName.FileName);
                    BrandImageName.UploadAsync(Path.Combine(_hostingEnvironment.WebRootPath, "images", "brands", brand.BrandImageName));

                    await _brandRepository.AddAsync(brand);
                    await _brandRepository.SaveChangesAsync();
                }
            }

            return RedirectToAction("Brands", new Brands());
        }

        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var brand = await _brandRepository.FindAsync(id);
            brand.RemovedAt = DateTime.Now;
            _brandRepository.Modify(brand);
            await _brandRepository.SaveChangesAsync();
            return RedirectToAction("Brands", new Brands());
        }

        public async Task<IActionResult> ProductBrand(Guid id)
        {
            var product = await _productRepository.GetProductWithBrandAsync(id);
            ViewBag.Brands = await _brandRepository.GetAllBrandsAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> SetProductBrand(ProductBrand brand)
        {

            await _productRepository.AddProductBrandAsync(new ProductBrand
            {
                ProductID = brand.ProductID,
                BrandID = brand.BrandID
            });

            try
            {
                await _productRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ProductBrand", new { id = brand.ProductID });
        }

        public async Task<IActionResult> EditProductBrand(Guid id)
        {
            var product = await _productRepository.GetProductWithBrandAsync(id);
            ViewBag.Brands = await _brandRepository.GetAllBrandsAsync();
            return PartialView(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductBrand(ProductBrand brand)
        {
            if (true)
            {
                var product = await _productRepository.GetProductWithBrandAsync(brand.Id);
                _productRepository.RemoveProdyctBrandAsync(product.ProductBrand.ToArray());
                await SetProductBrand(brand);
                await _productRepository.SaveChangesAsync();
            }

            return RedirectToAction("ProductBrand", new { id = brand.ProductID });
        }

        #endregion

        public async Task<IActionResult> SpecialProducts()
        {
            return View(await _productRepository.GetSpecialProductsAsync(true));
        }

        public async Task<IActionResult> CreateSpecialProduct()
        {
            ViewBag.Products = await _productRepository.GetAllProductsAsync(2000, 0);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialProduct(SpecialProducts sp)
        {
            await _productRepository.AddSpecialProductAsync(sp);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction("SpecialProducts", await _productRepository.GetSpecialProductsAsync(true));
        }

        public async Task<IActionResult> DeleteSepcialProduct(Guid id)
        {
            await _productRepository.RemoveSpecialProductAsync(id);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction("SpecialProducts", await _productRepository.GetSpecialProductsAsync(true));
        }

        //public Task<IActionResult> AcceptProducts()
        //{
        //    ViewBag.Sellers = db.Sellers.ToList();
        //    return View();
        //}

        //public Task<IActionResult> AcceptProductsTable()
        //{
        //    List<Products> products = db.Products.Where(p => p.IsAcceptedByAdmin == false).ToList();
        //    return PartialView(products);
        //}

        //public Task<IActionResult> AcceptProduct(int id)
        //{
        //    Products product = db.Products.Find(id);
        //    product.IsAcceptedByAdmin = true;
        //    db.Entry(product).State = EntityState.Modified;
        //    db.SaveChanges();

        //    List<Products> products = db.Products.Where(p => p.IsAcceptedByAdmin == false).ToList();
        //    return RedirectToAction("AcceptProducts", products);
        //}

        public async Task<IActionResult> DeactiveProduct(Guid id)
        {
            var product = await _productRepository.FindAsync(id);
            product.IsActive = false;
            _productRepository.Modify(product);
            await _productRepository.SaveChangesAsync();

            return PartialView("ProductsList", await _productRepository.GetAllProductsAsync(20, 0));
        }

        public async Task<IActionResult> ActivateProduct(Guid id)
        {
            var product = await _productRepository.FindAsync(id);
            product.IsActive = true;
            _productRepository.Modify(product);
            await _productRepository.SaveChangesAsync();

            return PartialView("ProductsList", await _productRepository.GetAllProductsAsync(20, 0));
        }

        //public Task<IActionResult> SortByVisit(bool acc)
        //{
        //    List<Products> products = db.Products.ToList();
        //    if (!acc)
        //    {
        //        products = products.OrderByDescending(p => p.Visit).ToList();
        //    }
        //    else
        //    {
        //        products = products.OrderBy(p => p.Visit).ToList();
        //    }
        //    return PartialView("ProductsList", products);
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