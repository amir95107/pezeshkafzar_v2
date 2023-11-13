using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class OrderDetails : BaseEntity
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalDiscount { get; set; }
        public int Condition { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}
