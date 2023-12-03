using DataLayer.Models.Base;

namespace DataLayer.Models
{

    public partial class Product_Galleries : GuidAuditableEntity
    {
        public Guid ProductID { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        //public bool IsMain { get; set; }

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
