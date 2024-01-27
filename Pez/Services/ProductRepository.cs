using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;
using System.Linq.Expressions;

namespace Pezeshkafzar_v2.Services
{
    public class ProductRepository : BaseRepository<Products, Guid>, IProductRepository
    {
        private readonly IQueryable<Products> NotRemoved;
        private readonly DbSet<Features> _features;
        private readonly DbSet<Product_Groups> _productGroups;
        private readonly DbSet<Product_Selected_Groups> _selectedGroups;
        private readonly DbSet<Product_Tags> _productTags;
        private readonly DbSet<Product_Galleries> _productGalleries;
        private readonly DbSet<Product_Features> _productFeatures;
        private readonly DbSet<ProductBrand> _productBrands;
        private readonly DbSet<SpecialProducts> _specialProducts;
        public ProductRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
            _productGroups = context.Set<Product_Groups>();
            _selectedGroups = context.Set<Product_Selected_Groups>();
            _productTags = context.Set<Product_Tags>();
            _productGalleries = context.Set<Product_Galleries>();
            _features = context.Set<Features>();
            _productFeatures = context.Set<Product_Features>();
            _productBrands = context.Set<ProductBrand>();
            _specialProducts = context.Set<SpecialProducts>();
        }

        public async Task<List<Products>> GetAcceptedProductsAsync()
            => await NotRemoved.Where(x => x.IsAcceptedByAdmin && x.IsActive).ToListAsync();

        public async Task<List<Products>> GetAllProductsWithDiscountAsync(int take, int skip)
            => await NotRemoved
                .Where(x => x.IsAcceptedByAdmin && x.Price != x.PriceAfterDiscount && x.PriceAfterDiscount > 0)
                .OrderByDescending(x => (x.Price - x.PriceAfterDiscount) / x.Price)
                .Take(take)
                .Skip(skip)
                .ToListAsync();

        public async Task<List<Products>> GetBestSellingsProductsAsync()
        => await NotRemoved
            .Include(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .Where(x => x.IsAcceptedByAdmin && x.IsInBestselling).ToListAsync();



        public async Task<List<Features>> GetAllFeaturesAsync()
        => await _features
        .Where(x => x.RemovedAt == null)
        .ToListAsync();

        public async Task<List<Features>> GetFeaturesAsync(Guid productId)
            => await Entities
            .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
            .ThenInclude(x => x.Features)
            .Where(x => x.Id == productId)
            .SelectMany(x => x.Product_Features)
            .Select(x => x.Features)
            .ToListAsync();

        public async Task<List<Features>> GetFeaturesAsync(string uniqueKey)
         => await Entities
            .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
            .ThenInclude(x => x.Features)
            .Where(x => x.UniqueKey == uniqueKey)
            .SelectMany(x => x.Product_Features)
            .Select(x => x.Features)
            .ToListAsync();

        public async Task<List<Products>> GetLastAddedProductsAsync(int take)
            => await NotRemoved
            .Include(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .OrderByDescending(x => x.CreateDate).Take(take).ToListAsync();

        public async Task<List<Products>> GetLikedProductsAsync(Guid userId)
            => await Entities
            .Include(x => x.LikeProduct.Where(x => x.UserId == userId))
            .Where(x => x.LikeProduct.Any())
            .ToListAsync();

        public async Task<Dictionary<string, decimal>> GetMinMaxPriceOfAllProducts()
        {
            var minPrice = await Entities.Select(x => x.Price).MinAsync();
            var maxPrice = await Entities.Select(x => x.Price).MaxAsync();

            return new Dictionary<string, decimal>
            {
                {"minPrice",minPrice },
                {"maxPrice",maxPrice }
            };
        }

        public async Task<List<Brands>> GetProductBrandsAsync(Guid productId, int take)
            => await Entities
            .Include(x => x.ProductBrand.Where(x => x.RemovedAt == null))
            .ThenInclude(x => x.Brands)
            .SelectMany(x => x.ProductBrand)
            .Where(x => x.ProductID == productId)
            .Select(x => x.Brands)
            .Take(take)
            .ToListAsync();

        public async Task<List<Brands>> GetProductBrandsAsync(string uniqueKey, int take)
            => await Entities
            .Include(x => x.ProductBrand.Where(x => x.RemovedAt == null))
            .ThenInclude(x => x.Brands)
            .SelectMany(x => x.ProductBrand)
            .Where(x => x.Products.UniqueKey == uniqueKey)
            .Select(x => x.Brands)
            .Take(take)
            .ToListAsync();

        public async Task<List<Comments>> GetProductCommentsAsync(Guid productId)
            => await Entities
                .Include(x => x.Comments.Where(x => x.RemovedAt == null))
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Comments)
                .ToListAsync();

        public async Task<List<Comments>> GetProductCommentsAsync(string uniqueKey)
        => await Entities
                .Include(x => x.Comments.Where(x => x.RemovedAt == null))
                .Where(x => x.UniqueKey == uniqueKey)
                .SelectMany(x => x.Comments)
                .ToListAsync();

        public async Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId)
            => await Entities
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Product_Features)
                .ToListAsync();

        public async Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, Guid featureId)
        => await Entities
            .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Product_Features.Where(xx => xx.Id == featureId))
                .ToListAsync();

        public async Task<List<Product_Galleries>> GetProductGalleriesAsync(Guid productId)
        {
            var a = await Entities
                .Include(x => x.Product_Galleries.Where(x => x.RemovedAt == null))
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Product_Galleries.Where(x => x.RemovedAt == null))
                .ToListAsync();
            return a;
        }

        public async Task<List<Product_Galleries>> GetProductGalleriesAsync(string uniqueKey)
        => await Entities
                .Include(x => x.Product_Galleries.Where(x => x.RemovedAt == null))
                .Where(x => x.UniqueKey == uniqueKey)
                .SelectMany(x => x.Product_Galleries)
                .ToListAsync();

        public async Task<List<Product_Groups>> GetProductGroupsAsync(bool justParent)
        {
            return await _productGroups
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .Where(x => !x.IsDeleted/* && justParent ? x.ParentID == null : true*/)
                .ToListAsync();
        }

        public async Task<List<Product_Groups>> GetProductGroupsAsync(Guid parentId)
        => await _productGroups
            .Where(x => x.ParentID == parentId && x.RemovedAt != null)
            .ToListAsync();

        public async Task<List<Product_Groups>> GetProductGroupsAsync(Guid? parentId, Guid? groupId)
        => await _productGroups
            .Where(x => x.ParentID == parentId && x.Id == groupId && !x.IsDeleted)
            .ToListAsync();

        public async Task<List<Products>> GetProductListAsync(int take, int skip, string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId)
        {
            var products = NotRemoved
                .Include(x => x.Product_Selected_Groups)
                .ThenInclude(x => x.Product_Groups)
                .Where(x => x.IsAcceptedByAdmin && x.IsActive);
            if (!string.IsNullOrWhiteSpace(title))
                products = products.Where(x => x.Title.Contains(title) || x.Text.Contains(title));
            if (minPrice > 0)
                products = products.Where(x => x.Price >= minPrice);
            if (maxPrice > 0)
                products = products.Where(x => x.Price <= maxPrice);
            if (brandId != null)
                products = products.Include(x => x.ProductBrand)
                    .Where(x => x.ProductBrand.Any(xx => xx.Id == brandId));
            if (selectedGroups != null)
            {
                products = products
                    .Include(x => x.Product_Selected_Groups.Where(xx => selectedGroups.Contains(xx.GroupID)))
                    .Where(x => x.Product_Selected_Groups.Any(xx => selectedGroups.Contains(xx.GroupID)));
            }
            try
            {
                return await products
                .Take(take)
                .Skip(skip)
                .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            };
        }

        public async Task<int> GetProductListCountAsync(string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId)
        {
            var products = NotRemoved
                .Where(x => x.IsAcceptedByAdmin && x.IsActive);
            if (!string.IsNullOrWhiteSpace(title))
                products = products.Where(x => x.Title.Contains(title) || x.Text.Contains(title));
            if (minPrice > 0)
                products = products.Where(x => x.Price >= minPrice);
            if (maxPrice > 0)
                products = products.Where(x => x.Price <= maxPrice);
            if (brandId != null)
                products = products.Include(x => x.ProductBrand)
                    .Where(x => x.ProductBrand.Any(xx => xx.Id == brandId));
            if (selectedGroups != null)
            {
                products = products
                    .Include(x => x.Product_Selected_Groups.Where(xx => selectedGroups.Contains(xx.GroupID)))
                    .Where(x => x.Product_Selected_Groups.Any(xx => selectedGroups.Contains(xx.GroupID)));
            }
            return await products
                .CountAsync();
        }

        public Task<List<Product_Groups>> GetProductSelectedGroupsAsync(Guid parentId)
        {
            return Task.FromResult(new List<Product_Groups>());
        }

        public async Task<List<Products>> GetProductSelectedGroupsAsync(List<Guid> groupids)
            => await _selectedGroups
                .Include(x => x.Products)
                .Where(x => groupids.Contains(x.GroupID))
                .Select(x => x.Products)
                .ToListAsync();

        public async Task<List<Products>> GetProductTitleAndImageAsync(Guid productId)
            => await Entities.Where(x => x.Id == productId).Select(x => new Products
            {
                Title = x.Title,
                ImageName = x.ImageName
            }).ToListAsync();

        public async Task<List<Products>> GetProductTitleAndImageAsync(string uniqueKey)
            => await Entities.Where(x => x.UniqueKey == uniqueKey).Select(x => new Products
            {
                Title = x.Title,
                ImageName = x.ImageName
            }).ToListAsync();

        public async Task<List<Products>> GetRelatedProductsAsync(Guid productId, int take)
        {
            var gid = _productGroups.Include(x => x.Product_Selected_Groups.Any(xx => xx.ProductID == productId)).Select(p => p.Id).Min();
            var products = _productGroups
                .Include(x => x.Product_Selected_Groups)
                .ThenInclude(x => x.Products)
                .Where(p => p.Id == gid && !p.Product_Selected_Groups.Any(xx => xx.GroupID == gid))
                .SelectMany(p => p.Product_Selected_Groups)
                .Select(x => x.Products)
                .Where(p => p.IsActive == true && p.IsAcceptedByAdmin && p.RemovedAt == null)
                .Take(take);

            return await products.ToListAsync();
        }

        public Task<List<Products>> GetRelatedProductsAsync(string uniqueKey)
        {
            return Task.FromResult(new List<Products>());
        }

        public async Task<List<SpecialProducts>> GetSpecialProductsAsync(bool forPanel)
            => await _specialProducts
            .Include(x => x.Products)
            .ThenInclude(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .Where(x => !forPanel ? x.IsActive && x.CreatedAt <= DateTime.Now && x.ExpireDate >= DateTime.Now : true)
            .ToListAsync();

        public async Task<bool> IsProductLiked(Guid userId, Guid productId)
            => await Entities.Include(x => x.LikeProduct.Where(xx => xx.ProductID == productId && xx.UserId == userId))
            .Where(x => x.LikeProduct.Any(xx => xx.ProductID == productId && xx.UserId == userId))
            .AnyAsync();

        public async Task<Products> ProductDetailAsync(string uniqueKey)
            => await Entities
            .Include(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .Include(x => x.Product_Tags)
            .Include(x => x.ProductBrand)
            .Include(x => x.Product_Features)
            .Include(x => x.Product_Galleries)
            .FirstOrDefaultAsync(x => x.UniqueKey == uniqueKey);

        public async Task<Products> ProductDetailAsync(Guid id)
            => await Entities
            .Include(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .Include(x => x.Product_Tags)
            .Include(x => x.ProductBrand)
            .Include(x => x.Product_Features)
            .Include(x => x.Product_Galleries)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Products>> GetAllProductsAsync()
            => await Entities
            .Include(x=>x.Product_Selected_Groups)
            .ThenInclude(x=>x.Product_Groups)
            .OrderByDescending(x=>x.CreatedAt)
            .ToListAsync();

        public async Task<Product_Groups> GetGroupAsync(Guid id)
            => await _productGroups.FindAsync(id);

        public async Task AddGroupAsync(Product_Groups group)
            => await _productGroups.AddAsync(group);

        public async Task<Products> FindProductWithChildrenAsync(Guid id)
            => await Entities
                .Include(x => x.Product_Selected_Groups)
                .Include(x => x.ProductBrand)
                .Include(x => x.Product_Features)
                .Include(x => x.Product_Tags)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Product_Tags>> GetTagsAsync(string tag)
            => await _productTags.Where(x => x.Tag.Contains(tag)).Take(10).ToListAsync();

        public async Task<Product_Galleries> GetProductGalleryAsync(Guid id)
            => await _productGalleries.Where(x => x.RemovedAt == null).Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Products> GetProductWithGalleriesAsync(Guid id)
            => await NotRemoved
            .Include(x => x.Product_Galleries.Where(x => x.RemovedAt == null))
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Products> GetProductWithFeaturesAsync(Guid id)
            => await Entities
                .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<Products> GetFeatureWithProductAsync(Guid featureId)
            => await Entities
                .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
                .Where(x => x.Product_Features.Any(x => x.FeatureID == featureId))
                .FirstOrDefaultAsync();

        public async Task<Products> FindAsync<TModel>(Guid id, params Expression<Func<TModel, object>>[] includes)
            => await NotRemoved
            .Include(x => x.Product_Features.Where(x => x.RemovedAt == null))
            .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<Products> GetProductWithBrandAsync(Guid id)
        => await NotRemoved
            .Include(x => x.ProductBrand.Where(x => x.RemovedAt == null))
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        public async Task<Features> GetFeatureAsync(Guid featureId)
            => await _features
            .FirstOrDefaultAsync(x => x.Id == featureId);

        public async Task<int?> GetLastGroupNumberAsync()
            => await _productGroups.MaxAsync(x => (int?)x.UniqueKey);

        public async Task<Guid?> GetGroupParentIdByKeyAsync(int value)
            => await _productGroups
            .Where(x => x.UniqueKey == value)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        public async Task<Products> GetGalleryWithproductAsync(Guid id)
            => await Entities
            .Include(x => x.Product_Galleries.Where(x => x.RemovedAt == null))
            .Where(x => x.Product_Galleries.Any(xx => xx.Id == id))
            .FirstOrDefaultAsync();

        public async Task AddProductFeature(Product_Features feature)
            => await _productFeatures.AddAsync(feature);

        public void RemoveProductFeatures(Product_Features[] features)
         => _productFeatures.RemoveRange(features);

        public async Task RemoveProductFeatures(Guid[] featureIds)
        {
            var features = new List<Product_Features>();
            foreach (var featureId in featureIds)
            {
                var f = await _productFeatures.FindAsync(featureId);
                if (f is null)
                    continue;

                features.Add(f);
            }
            _productFeatures.RemoveRange(features.ToArray());
        }

        public async Task AddProductBrandAsync(ProductBrand brand)
            => await _productBrands.AddAsync(brand);

        public void RemoveProdyctBrandAsync(ProductBrand[] brand)
            => _productBrands.RemoveRange(brand);

        public async Task RemoveProdyctBrandAsync(Guid[] brands)
        {
            var ProductBrand = new List<ProductBrand>();
            foreach (var brand in brands)
            {
                var f = await _productBrands.FindAsync(brand);
                if (f is null)
                    continue;

                ProductBrand.Add(f);
            }
            _productBrands.RemoveRange(ProductBrand.ToArray());
        }

        public async Task AddSpecialProductAsync(SpecialProducts special)
            => await _specialProducts.AddAsync(special);

        public void RemoveSpecialProductAsync(SpecialProducts special)
        => _specialProducts.Remove(special);

        public async Task RemoveSpecialProductAsync(Guid id)
        {
            var special = await _specialProducts.FindAsync(id);

            if (special is null)
                throw new Exception("special not found");

            RemoveSpecialProductAsync(special);
        }

        public async Task<List<Products>> GetAllAsync(IEnumerable<Guid> ids)
            => await Entities
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        public async Task<List<Products>> GetSpecialOffersAsync()
            => await NotRemoved
            .Include(x => x.Product_Selected_Groups)
            .ThenInclude(x => x.Product_Groups)
            .Where(x => x.Price > x.PriceAfterDiscount && x.IsActive)
            .ToListAsync();

        public async Task<List<Products>> GetProductsByTagsAsync(string q)
            => await Entities
            .Include(x => x.Product_Tags.Where(xx => xx.Tag == q))
            .Where(x => x.Product_Tags.Any(xx => xx.Tag == q) && x.IsAcceptedByAdmin && x.IsActive)
            .ToListAsync();
    }
}
