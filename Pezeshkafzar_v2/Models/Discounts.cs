namespace DataLayer
{
    using Pezeshkafzar_v2.Models;

    public partial class Discounts : BaseEntity
    {
        public Guid UserID { get; set; }
        public string DiscountCode { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsUsed { get; set; }
        public decimal MaxDiscountValue { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
