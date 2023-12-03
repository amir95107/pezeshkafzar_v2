namespace DataLayer.Models
{
    using DataLayer.Models.Base;

    public partial class Product_Tags : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public string Tag { get; set; }

        public virtual Products Products { get; set; }

        public Product_Tags()
        {
            Id= Guid.NewGuid();
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
