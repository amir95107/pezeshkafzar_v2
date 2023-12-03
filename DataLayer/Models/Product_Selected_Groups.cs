namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System;

    public partial class Product_Selected_Groups: GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public Guid GroupID { get; set; }
    
        public virtual Product_Groups Product_Groups { get; set; }
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
    }
}
