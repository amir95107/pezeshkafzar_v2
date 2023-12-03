namespace DataLayer.Models
{
    using System;
    using System.Collections.Generic;
    using DataLayer.Models.Base;

    public partial class Products : GuidAuditableAggregateRoot
    {
        public string UniqueKey { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public int LikeCount { get; set; }
        public int Stock { get; set; }
        public double? Point { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool IsSpecial { get; set; }
        public string SefUrl { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? Visit { get; set; }
        public string Garanty { get; set; }
        public int? MinCount { get; set; }
        public bool IsInBestselling { get; set; }
        public int ShowOrder { get; set; }

        public virtual ICollection<LikeProduct> LikeProduct { get; set; } = new HashSet<LikeProduct>();
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
        public virtual ICollection<Product_Features> Product_Features { get; set; } = new HashSet<Product_Features>();
        public virtual ICollection<Product_Galleries> Product_Galleries { get; set; } = new HashSet<Product_Galleries>();
        public virtual ICollection<Product_Selected_Groups> Product_Selected_Groups { get; set; } = new HashSet<Product_Selected_Groups>();
        public virtual ICollection<Product_Tags> Product_Tags { get; set; } = new HashSet<Product_Tags>();
        public virtual ICollection<ProductBrand> ProductBrand { get; set; } = new HashSet<ProductBrand>();
        public virtual ICollection<Comments> Comments { get; set; } = new HashSet<Comments>();
    }
}
