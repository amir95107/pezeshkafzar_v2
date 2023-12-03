using DataLayer.Models.Base;

namespace DataLayer.Models
{
    public partial class ProductBrand : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public Guid BrandID { get; set; }

        public virtual Brands Brands { get; set; }
        public virtual Products Products { get; set; }

        public ProductBrand()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        protected override void EnsureReadyState(object @event)
        {
            throw new NotImplementedException();
        }

        protected override void EnsureValidState()
        {
            throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}
