using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IProductRepository : IBaseRepository<Products, Guid>
    {
        Task<List<Features>> GetFeaturesAsync(Guid productId, CancellationToken cancellationToken);
        Task<List<Features>> GetFeaturesAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, CancellationToken cancellationToken);
        Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, Guid featureId, CancellationToken cancellationToken);
        Task<List<Products>> GetProductTitleAndImageAsync(Guid productId, CancellationToken cancellationToken);
        Task<List<Products>> GetProductTitleAndImageAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Product_Groups>> GetProductGroupsAsync(bool justParent,CancellationToken cancellationToken);
        Task<List<Product_Groups>> GetProductGroupsAsync(Guid parentId, CancellationToken cancellationToken);
        Task<List<Product_Groups>> GetProductGroupsAsync(Guid? parentId,Guid? groupId, CancellationToken cancellationToken);
        Task<List<Product_Groups>> GetProductSelectedGroupsAsync(Guid parentId, CancellationToken cancellationToken);
        Task<List<Products>> GetProductSelectedGroupsAsync(List<Guid> groupsIds, CancellationToken cancellationToken);
        Task<List<Products>> GetLastAddedProductsAsync(int take, CancellationToken cancellationToken);
        Task<List<Products>> GetAllProductsWithDiscountAsync(int take, int skip, CancellationToken cancellationToken);
        Task<Products> ProductDetailAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Product_Galleries>> GetProductGalleriesAsync(Guid productId, CancellationToken cancellationToken);
        Task<List<Product_Galleries>> GetProductGalleriesAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Products>> GetLikedProductsAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsProductLiked(Guid userId, Guid productId, CancellationToken cancellationToken);
        Task<List<Comments>> GetProductCommentsAsync(Guid productId, CancellationToken cancellationToken);
        Task<List<Comments>> GetProductCommentsAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Products>> GetProductListAsync(int take, int skip, string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId, CancellationToken cancellationToken);
        Task<int> GetProductListCountAsync(string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId, CancellationToken cancellationToken);
        Task<List<Products>> GetAcceptedProductsAsync(CancellationToken cancellationToken);
        Task<List<Products>> GetRelatedProductsAsync(Guid productId, int take, CancellationToken cancellationToken);
        Task<List<Products>> GetRelatedProductsAsync(string uniqueKey, CancellationToken cancellationToken);
        Task<List<Brands>> GetProductBrandsAsync(Guid productId, int take, CancellationToken cancellationToken);
        Task<List<Brands>> GetProductBrandsAsync(string uniqueKey, int take, CancellationToken cancellationToken);
        Task<List<Brands>> GetAllBrandsAsync(CancellationToken cancellationToken);
        Task<List<Products>> GetSpecialProductsAsync(CancellationToken cancellationToken);
        Task<List<Products>> GetBestSellingsProductsAsync(CancellationToken cancellationToken);
        Task<Dictionary<string, decimal>> GetMinMaxPriceOfAllProducts(CancellationToken cancellationToken);
    }
}