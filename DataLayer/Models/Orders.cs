namespace DataLayer.Models
{
    using DataLayer.Models.Base;
    using System;
    using System.Collections.Generic;

    public partial class Orders : GuidAuditableAggregateRoot
    {
        public string TraceCode { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Payable { get; set; }
        public bool IsFinaly { get; set; }
        public int PaymentWay { get; set; }
        public bool UseDiscountCode { get; set; }
        public bool IsSent { get; set; }
        public Guid? DeliveryID { get; set; }
        public decimal? DeliveryPrice { get; set; }

        public virtual DeliveryWays DeliveryWays { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
        public virtual Users Users { get; set; }

        public Orders()
        {
            Id= Guid.NewGuid();
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
