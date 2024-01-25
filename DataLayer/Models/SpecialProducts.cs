using DataLayer.Models.Base;

namespace DataLayer.Models
{
    public class SpecialProducts : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }

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
