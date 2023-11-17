namespace DataLayer.Models
{
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;

    public partial class Orders : BaseEntity
    {
        public string TraceCode { get; set; }
        public Guid UserID { get; set; }
        public DateTime Date { get; set; }
        public int Payable { get; set; }
        public bool IsFinaly { get; set; }
        public int PaymentWay { get; set; }
        public bool UseDiscountCode { get; set; }
        public bool IsSent { get; set; }
        public Guid? DeliveryID { get; set; }
        public decimal? DeliveryPrice { get; set; }

        public virtual DeliveryWays DeliveryWays { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
        public virtual Users Users { get; set; }
    }
}
