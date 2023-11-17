using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class ProductRepository : BaseRepository<Products, Guid>, IProductRepository
    {
        private readonly IQueryable<Products> NotRemoved;
        private readonly DbSet<Brands> _brands;
        private readonly DbSet<Product_Features> _productFeatures;
        private readonly DbSet<Product_Groups> _productGroups;
        private readonly DbSet<Product_Selected_Groups> _selectedGroups;
        public ProductRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            _brands = context.Set<Brands>();
            NotRemoved = Entities.Where(x => x.RemovedAt == null);
            _productFeatures = context.Set<Product_Features>();
            _productGroups = context.Set<Product_Groups>();
            _selectedGroups = context.Set<Product_Selected_Groups>();
        }

        public async Task<List<Products>> GetAcceptedProductsAsync(CancellationToken cancellationToken)
            => await NotRemoved.Where(x => x.IsAcceptedByAdmin && x.IsActive).ToListAsync(cancellationToken);

        public async Task<List<Brands>> GetAllBrandsAsync(CancellationToken cancellationToken)
            => await _brands.ToListAsync(cancellationToken);

        public async Task<List<Products>> GetAllProductsWithDiscountAsync(int take, int skip, CancellationToken cancellationToken)
            => await NotRemoved
                .Where(x => x.IsAcceptedByAdmin && x.Price != x.PriceAfterDiscount && x.PriceAfterDiscount > 0)
                .OrderByDescending(x => (x.Price - x.PriceAfterDiscount) / x.Price)
                .Take(take)
                .Skip(skip)
                .ToListAsync(cancellationToken);

        public async Task<List<Products>> GetBestSellingsProductsAsync(CancellationToken cancellationToken)
        => await NotRemoved.Where(x => x.IsAcceptedByAdmin && x.IsInBestselling).ToListAsync(cancellationToken);

        public Task<List<Brands>> GetAllBrandAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Features>> GetFeaturesAsync(Guid productId, CancellationToken cancellationToken)
            => await _productFeatures
            .Include(x => x.Products)
            .Include(x => x.Features)
            .Where(x => x.Products.Id == productId)
            .Select(x => x.Features)
            .ToListAsync(cancellationToken);

        public async Task<List<Features>> GetFeaturesAsync(string uniqueKey, CancellationToken cancellationToken)
         => await _productFeatures
            .Include(x => x.Products)
            .Include(x => x.Features)
            .Where(x => x.Products.UniqueKey == uniqueKey)
            .Select(x => x.Features)
            .ToListAsync(cancellationToken);

        public async Task<List<Products>> GetLastAddedProductsAsync(int take, CancellationToken cancellationToken)
            => await NotRemoved.OrderByDescending(x => x.CreateDate).Take(take).ToListAsync(cancellationToken);

        public async Task<List<Products>> GetLikedProductsAsync(Guid userId, CancellationToken cancellationToken)
            => await Entities
            .Include(x => x.LikeProduct.Where(x => x.UserID == userId))
            .Where(x => x.LikeProduct.Any())
            .ToListAsync(cancellationToken);

        public async Task<Dictionary<string, decimal>> GetMinMaxPriceOfAllProducts(CancellationToken cancellationToken)
        {
            var minPrice = await Entities.Select(x => x.Price).MinAsync(cancellationToken);
            var maxPrice = await Entities.Select(x => x.Price).MaxAsync(cancellationToken);

            return new Dictionary<string, decimal>
            {
                {"minPrice",minPrice },
                {"maxPrice",maxPrice }
            };
        }

        public async Task<List<Brands>> GetProductBrandsAsync(Guid productId, int take, CancellationToken cancellationToken)
            => await Entities
            .Include(x => x.ProductBrand)
            .ThenInclude(x => x.Brands)
            .SelectMany(x => x.ProductBrand)
            .Where(x => x.ProductID == productId)
            .Select(x => x.Brands)
            .Take(take)
            .ToListAsync(cancellationToken);

        public async Task<List<Brands>> GetProductBrandsAsync(string uniqueKey, int take, CancellationToken cancellationToken)
            => await Entities
            .Include(x => x.ProductBrand)
            .ThenInclude(x => x.Brands)
            .SelectMany(x => x.ProductBrand)
            .Where(x => x.Products.UniqueKey == uniqueKey)
            .Select(x => x.Brands)
            .Take(take)
            .ToListAsync(cancellationToken);

        public async Task<List<Comments>> GetProductCommentsAsync(Guid productId, CancellationToken cancellationToken)
            => await Entities
                .Include(x => x.Comments)
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Comments)
                .ToListAsync(cancellationToken);

        public async Task<List<Comments>> GetProductCommentsAsync(string uniqueKey, CancellationToken cancellationToken)
        => await Entities
                .Include(x => x.Comments)
                .Where(x => x.UniqueKey == uniqueKey)
                .SelectMany(x => x.Comments)
                .ToListAsync(cancellationToken);

        public async Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, CancellationToken cancellationToken)
            => await _productFeatures
                .Where(x => x.Products.Id == productId)
                .ToListAsync(cancellationToken);

        public async Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, Guid featureId, CancellationToken cancellationToken)
        => await _productFeatures
                .Where(x => x.Products.Id == productId && x.FeatureID == featureId)
                .ToListAsync(cancellationToken);

        public async Task<List<Product_Galleries>> GetProductGalleriesAsync(Guid productId, CancellationToken cancellationToken)
        => await Entities
                .Include(x => x.Product_Galleries)
                .Where(x => x.Id == productId)
                .SelectMany(x => x.Product_Galleries)
                .ToListAsync(cancellationToken);

        public async Task<List<Product_Galleries>> GetProductGalleriesAsync(string uniqueKey, CancellationToken cancellationToken)
        => await Entities
                .Include(x => x.Product_Galleries)
                .Where(x => x.UniqueKey == uniqueKey)
                .SelectMany(x => x.Product_Galleries)
                .ToListAsync(cancellationToken);

        public async Task<List<Product_Groups>> GetProductGroupsAsync(bool justParent,CancellationToken cancellationToken)
        {
            return await _productGroups.Where(x => !x.IsDeleted && justParent ? x.ParentID == null : true).ToListAsync(cancellationToken);
        }

        public async Task<List<Product_Groups>> GetProductGroupsAsync(Guid parentId, CancellationToken cancellationToken)
        => await _productGroups
            .Where(x => x.ParentID == parentId && x.RemovedAt != null)
            .ToListAsync(cancellationToken);

        public async Task<List<Product_Groups>> GetProductGroupsAsync(Guid? parentId, Guid? groupId, CancellationToken cancellationToken)
        => await _productGroups
            .Where(x => x.ParentID == parentId && x.Id == groupId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        public async Task<List<Products>> GetProductListAsync(int take, int skip, string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId, CancellationToken cancellationToken)
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
                .Take(take)
                .Skip(skip)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetProductListCountAsync(string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId, CancellationToken cancellationToken)
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
                .CountAsync(cancellationToken);
        }

        public Task<List<Product_Groups>> GetProductSelectedGroupsAsync(Guid parentId, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Product_Groups>());
        }

        public async Task<List<Products>> GetProductSelectedGroupsAsync(List<Guid> groupids, CancellationToken cancellationToken)
            => await _selectedGroups
                .Include(x => x.Products)
                .Where(x => groupids.Contains(x.GroupID))
                .Select(x => x.Products)
                .ToListAsync(cancellationToken);

        public async Task<List<Products>> GetProductTitleAndImageAsync(Guid productId, CancellationToken cancellationToken)
            => await Entities.Where(x => x.Id == productId).Select(x => new Products
            {
                Title = x.Title,
                ImageName = x.ImageName
            }).ToListAsync(cancellationToken);

        public async Task<List<Products>> GetProductTitleAndImageAsync(string uniqueKey, CancellationToken cancellationToken)
            => await Entities.Where(x => x.UniqueKey == uniqueKey).Select(x => new Products
            {
                Title = x.Title,
                ImageName = x.ImageName
            }).ToListAsync(cancellationToken);

        public async Task<List<Products>> GetRelatedProductsAsync(Guid productId, int take, CancellationToken cancellationToken)
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

            return await products.ToListAsync(cancellationToken);
        }

        public Task<List<Products>> GetRelatedProductsAsync(string uniqueKey, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Products>());
        }

        public async Task<List<Products>> GetSpecialProductsAsync(CancellationToken cancellationToken)
            => await Entities.Where(x => x.IsSpecial).ToListAsync(cancellationToken);

        public async Task<bool> IsProductLiked(Guid userId, Guid productId, CancellationToken cancellationToken)
            => await Entities.Include(x => x.LikeProduct.Where(xx => xx.ProductID == productId && xx.UserID == userId))
            .Where(x => x.LikeProduct.Any(xx => xx.ProductID == productId && xx.UserID == userId))
            .AnyAsync(cancellationToken);

        public async Task<Products> ProductDetailAsync(string uniqueKey, CancellationToken cancellationToken)
            => await Entities.FirstOrDefaultAsync(x => x.UniqueKey == uniqueKey, cancellationToken);
    }
}
