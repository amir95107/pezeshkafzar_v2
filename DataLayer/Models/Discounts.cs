namespace DataLayer.Models
{
    using DataLayer.Models.Base;

    public partial class Discounts : GuidAuditableAggregateRoot
    {
        public Guid UserId { get; set; }
        public string DiscountCode { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsUsed { get; set; }
        public decimal MaxDiscountValue { get; set; }
    
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
