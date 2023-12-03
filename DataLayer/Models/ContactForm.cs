using DataLayer.Models.Base;

namespace DataLayer.Models
{

    public partial class ContactForm : GuidAuditableEntity
    {
        public Guid? UserId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Answer { get; set; }
        public bool IsFaq { get; set; }

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
