using DataLayer.Models.Base;

namespace DataLayer.Models
{

    public partial class OrderDetails : GuidAuditableEntity
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalDiscount { get; set; }
        public int Condition { get; set; }
    
        public virtual Orders Orders { get; set; }
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
