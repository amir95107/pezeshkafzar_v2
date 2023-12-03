using DataLayer.Models.Events;

namespace DataLayer.Models
{
    public partial class Products
    {
        protected override void EnsureReadyState(object @event)
        {
        }

        protected override void EnsureValidState()
        {
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ProductGalleryChanged pga:

                    var list = Product_Galleries ?? new List<Product_Galleries>();

                    if (pga.EventType == EventType.Delete)
                    {
                        list.Single(x => x.Id == pga.GalleryId).RemovedAt = DateTime.Now;
                    }
                    else if (pga.EventType == EventType.Add)
                    {
                        list.Add(new Product_Galleries
                        {
                            Title = pga.Title,
                            ImageName = pga.ImageName,
                            ProductID = pga.ProductId
                        });
                    }

                    Product_Galleries = list;

                    break;

                case ProductBrandChanged pba:
                    var brandList = ProductBrand ?? new List<ProductBrand>();
                    //if (pba.EventType == EventType.Add)
                    //{
                    brandList.Add(new ProductBrand
                    {
                        BrandID = pba.BrandId,
                        ProductID = pba.ProductId
                    });
                    //}
                    //else if (pba.EventType == EventType.Delete)
                    //{
                    //    brandList = brandList.Where(x => x.BrandID != pba.BrandId).ToList();
                    //}
                    ProductBrand = brandList;
                    break;

                case ProductFeatureChanged pfa:
                    ModifiedAt = DateTime.Now;
                    Product_Features.Add(new Product_Features
                    {
                        FeatureID = pfa.FeatureId,
                        ProductID = pfa.ProductId,
                        Value = pfa.Value
                    });

                    break;

                case ProductTagChanged pta:

                    var listList = Product_Tags ?? new List<Product_Tags>();
                    //if (pta.EventType == EventType.Add)
                    //{
                    listList.Add(new Product_Tags
                    {
                        Tag = pta.Tag,
                        ProductID = pta.ProductId
                    });
                    //}
                    //else if (pta.EventType == EventType.Delete)
                    //{
                    //    listList = listList.Where(x => x.Tag != pta.Tag).ToList();
                    //}
                    Product_Tags = listList;

                    break;

                case ProductSelectedGroupChanged psa:
                    var gList = Product_Selected_Groups ?? new List<Product_Selected_Groups>();

                    //if (psa.EventType == EventType.Add)
                    //{
                    gList.Add(new Product_Selected_Groups
                    {
                        GroupID = psa.GroupId,
                        ProductID = psa.ProductId
                    });
                    //}
                    //else if (psa.EventType == EventType.Delete)
                    //{
                    //    gList = gList.Where(x => !(x.GroupID == psa.GroupId && x.ProductID == psa.ProductId)).ToList();
                    //}
                    Product_Selected_Groups = gList;
                    break;
            }
        }
    }
}
