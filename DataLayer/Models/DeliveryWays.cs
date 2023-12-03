namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System.Collections.Generic;

    public partial class DeliveryWays: GuidAuditableAggregateRoot
    {
        public string Title { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string Usage { get; set; }
        public bool PayByCustomer { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();

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
