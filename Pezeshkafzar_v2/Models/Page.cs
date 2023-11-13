namespace DataLayer
{
    using Pezeshkafzar_v2.Models;

    public partial class Page:BaseEntity
    {
        public string PageTitle { get; set; }
        public string PageContent { get; set; }
        public string HeadTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Url { get; set; }
    }
}
