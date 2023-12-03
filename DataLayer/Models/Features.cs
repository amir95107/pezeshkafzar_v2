namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System.Collections.Generic;

    public partial class Features : GuidAuditableAggregateRoot
    {
        public int FeatureID { get; set; }
        public string FeatureTitle { get; set; }

        public virtual ICollection<Product_Features> Product_Features { get; set; } = new HashSet<Product_Features>();

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
