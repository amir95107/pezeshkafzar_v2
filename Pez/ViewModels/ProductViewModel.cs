using DataLayer.Models;

namespace Pezeshkafzar_v2.ViewModels
{
    public class CreateProductViewModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public string ImageName { get; set; }
        public int Stock { get; set; }
        public string SefUrl { get; set; }
        public string Garanty { get; set; }
        public bool IsInBestselling { get; set; }
    }

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
