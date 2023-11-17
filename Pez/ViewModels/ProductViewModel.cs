using DataLayer.Models;

namespace Pezeshkafzar_v2.ViewModels
{
    public class ShowProductFeatureViewModel
    {
        public string FeatureTitle { get; set; }
        public List<string> Values { get; set; }
    }

    public class LastSeen
    {
        public Products product { get; set; }
    }

    public class BestSellingsInBlog
    {
        public Guid ProductID { get; set; }
        public string Title { get; set; }
        public string SefUrl { get; set; }
        public string Image { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

    }

}
