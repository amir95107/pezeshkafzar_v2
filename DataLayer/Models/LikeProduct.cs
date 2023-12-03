namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System;

    public partial class LikeProduct : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual Users Users { get; set; }

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
