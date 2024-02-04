using DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Pezeshkafzar_v2.ViewModels
{
    public class CreateProductViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal PriceAfterDiscount { get; set; }
        [Required]
        public string ImageName { get; set; }
        [Required]
        public int Stock { get; set; }
        
        public string SefUrl { get; set; }
        public string Garanty { get; set; }
        public bool IsInBestselling { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public int Visit { get; set; }
        public string UniqueKey { get; internal set; }
        public DateTime LastUpdated { get; internal set; }
        public int LikeCount { get; internal set; }
        public int Point { get; internal set; }
        public bool IsAcceptedByAdmin { get; internal set; }
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

    public class ProductGalleryViewModel
    {
        public Guid ProductID { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
    }

}
