using DataLayer.Models.Base;

namespace DataLayer.Models
{

    public partial class Slider : GuidAuditableEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

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
