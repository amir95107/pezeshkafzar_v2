namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;
    using System.Collections.Generic;

    public partial class Sellers : BaseEntity
    {

        public string SellerFullName { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Telephone { get; set; }
        public DateTime? StartDate { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public double? Point { get; set; }
        public Guid UserID { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public virtual ICollection<Products> Products { get; set; } = new HashSet<Products>();
        public virtual Users Users { get; set; }
    }
}
