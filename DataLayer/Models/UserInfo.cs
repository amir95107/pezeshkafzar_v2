using DataLayer.Models.Base;

namespace DataLayer.Models
{
    public partial class UserInfo : GuidAuditableEntity
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }

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
