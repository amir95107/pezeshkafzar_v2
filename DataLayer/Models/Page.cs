namespace DataLayer.Models
{
    using DataLayer.Models;

    public partial class Page:BaseEntity
    {
        public int PageKey { get; set; }
        public string PageTitle { get; set; }
        public string PageContent { get; set; }
        public string HeadTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Url { get; set; }
    }
}
