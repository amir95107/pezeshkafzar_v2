namespace DataLayer.Models
{
    using DataLayer.Models;
    using System.Collections.Generic;

    public partial class DeliveryWays: BaseEntity
    {
        public string Title { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string Usage { get; set; }
        public bool PayByCustomer { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();
    }
}
