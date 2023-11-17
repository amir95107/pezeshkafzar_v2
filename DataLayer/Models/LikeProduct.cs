namespace DataLayer.Models
{
    using DataLayer.Models;
    using System;

    public partial class LikeProduct : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid UserID { get; set; }
        public DateTime Date { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual Users Users { get; set; }
    }
}
