namespace DataLayer.Models
{
    using DataLayer.Models.Base;

    public partial class Page: GuidAuditableAggregateRoot
    {
        public int PageKey { get; set; }
        public string PageTitle { get; set; }
        public string PageContent { get; set; }
        public string HeadTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Url { get; set; }

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
