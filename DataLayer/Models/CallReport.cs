using DataLayer.Models.Base;

namespace DataLayer.Models
{

    public partial class CallReport: GuidAuditableAggregateRoot
    {
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public string Operator { get; set; }

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
