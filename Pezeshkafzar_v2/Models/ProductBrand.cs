using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class ProductBrand : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid BrandID { get; set; }

        public virtual Brands Brands { get; set; }
        public virtual Products Products { get; set; }
    }
}
