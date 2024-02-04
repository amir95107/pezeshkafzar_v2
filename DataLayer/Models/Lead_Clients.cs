namespace DataLayer.Models
{
    using DataLayer.Models.Base;

    public partial class Lead_Clients : GuidAuditableEntity
    {
        public string Mobile { get; set; }

        public Lead_Clients()
        {
            Id=Guid.NewGuid();
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
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
