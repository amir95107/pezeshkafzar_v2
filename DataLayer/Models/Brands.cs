namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System.Collections.Generic;

    public partial class Brands: GuidAuditableAggregateRoot
    {
        public string BrandTitle { get; set; }
        public string BrandImageName { get; set; }
        public virtual ICollection<ProductBrand> ProductBrand { get; set; } = new HashSet<ProductBrand>();
    }
}
