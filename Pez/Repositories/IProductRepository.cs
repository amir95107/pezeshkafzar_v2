using DataLayer.Models;
using System.Linq.Expressions;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IProductRepository : IBaseRepository<Products, Guid>
    {
        Task<Products> GetProductWithBrandAsync(Guid id);
        Task<Products> FindProductWithChildrenAsync(Guid id);
        Task<Products> GetProductWithFeaturesAsync(Guid id);
        Task<Features> GetFeatureAsync(Guid id);
        Task<Products> GetFeatureWithProductAsync(Guid featureId);        
        Task<List<Features>> GetAllFeaturesAsync();
        Task<List<Features>> GetFeaturesAsync(Guid productId);
        Task<List<Features>> GetFeaturesAsync(string uniqueKey);
        Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId);
        Task<List<Product_Features>> GetProductFeaturesAsync(Guid productId, Guid featureId);
        Task<List<Products>> GetProductTitleAndImageAsync(Guid productId);
        Task<List<Products>> GetProductTitleAndImageAsync(string uniqueKey);
        Task<List<Product_Groups>> GetProductGroupsAsync(bool justParent);
        Task<Product_Groups> GetGroupAsync(Guid id);
        Task AddProductBrandAsync(ProductBrand brand);
        void RemoveProdyctBrandAsync(ProductBrand[] brand);
        Task RemoveProdyctBrandAsync(Guid[] brand);
        Task AddGroupAsync(Product_Groups group);
        Task<List<Product_Groups>> GetProductGroupsAsync(Guid parentId);
        Task<List<Product_Groups>> GetProductGroupsAsync(Guid? parentId,Guid? groupId);
        Task<List<Product_Groups>> GetProductSelectedGroupsAsync(Guid parentId);
        Task<List<Products>> GetProductSelectedGroupsAsync(List<Guid> groupsIds);
        Task<List<Products>> GetLastAddedProductsAsync(int take);
        Task<List<Products>> GetAllProductsWithDiscountAsync(int take, int skip);
        Task<Products> ProductDetailAsync(Guid id);
        Task<Products> ProductDetailAsync(string uniqueKey);
        Task<List<Product_Galleries>> GetProductGalleriesAsync(Guid id);
        Task<Products> GetProductWithGalleriesAsync(Guid id);
        Task<Products> GetGalleryWithproductAsync(Guid id);
        Task<Product_Galleries> GetProductGalleryAsync(Guid productId);
        Task<List<Product_Galleries>> GetProductGalleriesAsync(string uniqueKey);
        Task<List<Products>> GetLikedProductsAsync(Guid userId);
        Task<bool> IsProductLiked(Guid userId, Guid productId);
        Task<List<Comments>> GetProductCommentsAsync(Guid productId);
        Task<List<Comments>> GetProductCommentsAsync(string uniqueKey);
        Task<List<Products>> GetProductListAsync(int take, int skip, string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId);
        Task<List<Products>> GetAllProductsAsync();
        Task<int> GetProductListCountAsync(string title, decimal minPrice, decimal maxPrice, List<Guid>? selectedGroups, Guid? brandId);
        Task<List<Products>> GetAcceptedProductsAsync( );
        Task<List<Products>> GetRelatedProductsAsync(Guid productId, int take);
        Task<List<Products>> GetRelatedProductsAsync(string uniqueKey);
        Task<List<Brands>> GetProductBrandsAsync(Guid productId, int take);
        Task<List<Brands>> GetProductBrandsAsync(string uniqueKey, int take);
        Task<List<Products>> GetSpecialProductsAsync( );
        Task<List<Products>> GetBestSellingsProductsAsync( );
        Task<Dictionary<string, decimal>> GetMinMaxPriceOfAllProducts();
        Task<List<Product_Tags>> GetTagsAsync(string tag);
        Task<int> GetLastGroupNumberAsync();
        Task<Guid?> GetGroupParentIdByKeyAsync(int value);
        Task AddProductFeature(Product_Features feature);
        void RemoveProductFeatures(Product_Features[] feature);
        Task RemoveProductFeatures(Guid[] featureIds);
    }
}