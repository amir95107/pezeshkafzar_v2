namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System.Collections.Generic;

    public partial class Brands: BaseEntity
    {
        public string BrandTitle { get; set; }
        public string BrandImageName { get; set; }
        public virtual ICollection<ProductBrand> ProductBrand { get; set; } = new HashSet<ProductBrand>();
    }
}
