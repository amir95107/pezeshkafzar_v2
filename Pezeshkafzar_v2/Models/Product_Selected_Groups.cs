namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;

    public partial class Product_Selected_Groups:BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid GroupID { get; set; }
    
        public virtual Product_Groups Product_Groups { get; set; }
        public virtual Products Products { get; set; }
    }
}
