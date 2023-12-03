namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System;

    public partial class Comments : GuidAuditableAggregateRoot
    {
        public Guid? ProductID { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentID { get; set; }
        public bool IsShow { get; set; }
        public Guid? BlogID { get; set; }

        public virtual Blogs Blogs { get; set; }
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
