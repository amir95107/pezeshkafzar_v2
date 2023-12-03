namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System;

    public partial class Product_Features : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public Guid FeatureID { get; set; }
        public string Value { get; set; }
        public string? ImageName { get; set; }

        public virtual Features Features { get; set; }
        public virtual Products Products { get; set; }

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

        public Product_Features()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
