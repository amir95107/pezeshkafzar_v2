namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System.Collections;
    using System.Collections.Generic;

    public partial class Product_Groups : GuidAuditableAggregateRoot
    {
        public int? UniqueKey { get; set; }
        public string GroupTitle { get; set; }
        public Guid? ParentID { get; set; }
        public bool IsDeleted { get; set; }
        public int ShowOrder { get; set; }

        public virtual Product_Groups Parent { get; set; }
        public virtual ICollection<Product_Groups> Children { get; set; }
        public virtual ICollection<Product_Selected_Groups> Product_Selected_Groups { get; set; } = new HashSet<Product_Selected_Groups>();

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
